import sys
import threading
from dataclasses import dataclass
import socket, struct, time
from tkinter import messagebox
import message as msg
from tkinter import *
import os


class MainWindow:
    def __init__(self):
        self.username = ""
        self.ActiveUsers = dict()
        self.root = Tk()
        self.frame1 = Frame(self.root)
        self.frame2 = Frame(self.root,bg="#00a86b")
        self.MsgList = Listbox(self.frame1, width=100, height=100, font=("Century gothic",12),bg="#FFDAB9")
        self.UsrList = Listbox(self.frame1, width=50, height=100, font=("Century gothic",12),bg="#E6E6FA")
        self.SendBtn = Button(self.frame2, text='SEND', command=self.btn_click, width=15, height=1,font="Arial 18 bold",bg="#cc7722", fg="white")
        self.MsgInput = Entry(self.frame2, width=170,borderwidth=5,font=("Century gothic",12))
        self.InitUI();

    def InitUI(self):
        self.root['bg'] = '#FFFAFA'
        self.root.title('PyClient')
        self.root.geometry('1200x430')
        self.root.resizable(width=False, height=False)

        self.frame1.pack(side=TOP)
        self.frame2.pack(side=BOTTOM)
        self.frame1.place(relwidth=1, relheight=0.8)
        self.frame2.place(relwidth=1, relheight=0.2,y=round(430 * 0.8))

        self.UsrList.pack(side=LEFT, fill=Y, expand=True)
        self.MsgList.pack(side=LEFT,fill=Y, expand=True)
        self.SendBtn.pack(side=RIGHT,padx=30)
        self.MsgInput.pack(side=LEFT,padx=30)

    def on_closing(self):
        if messagebox.askokcancel("Quit", "Do you want to quit?"):
            m = msg.Message.SendMessage(msg.MR_BROKER, msg.MT_EXIT)
            self.root.destroy()

    def btn_click(self):
        recipient = msg.MR_ALL
        message = self.MsgInput.get()
        isPrivate = False
        for key,value in self.ActiveUsers.items():
            if str(self.UsrList.get(ANCHOR)).find(value)!=-1:
                recipient = key
                self.MsgList.insert('end',f"You whispered to {value}: {message}")
                HistoryWrite(self.username,f"You whispered to {value}: {message}")
                isPrivate = True
                break
        if not isPrivate:
            self.MsgList.insert('end',f"You: {message}")
            HistoryWrite(self.username, f"You whispered to {value}: {message}")
        m = msg.Message.SendMessage(recipient, msg.MT_DATA, message)
        self.MsgInput.delete(0,'end')

    def RefreshUsers(self, str):
        self.ActiveUsers.clear()
        buf = str.split(' ')
        for number in range(0, len(buf) - 1, 2):
            self.ActiveUsers[int(buf[number])] = buf[number + 1]
        self.UsrList.delete(0,'end')
        self.UsrList.insert(0, 'All Users')
        for key, value in self.ActiveUsers.items():
            if self.username!=value:
                self.UsrList.insert(1, value)


class UsernameWindow:
    def __init__(self, app):
        self.app = app
        self.usrwnd = Toplevel(app.root)
        self.frame = Frame(self.usrwnd,bg="#00a86b")
        self.lbl = Label(self.usrwnd, text='ENTER NAME',font='Arial 14 bold', bg="#00cccc",fg="white")
        self.usrname = Entry(self.usrwnd,font=("Century gothic",14))
        self.lbl2 = Label(self.usrwnd, text='ENTER PASSWORD',font='Arial 14 bold', bg="#00cccc",fg="white")
        self.password = Entry(self.usrwnd ,font=("Century gothic",14),show="*")
        self.ackbtn = Button(self.usrwnd, command=self.AckUsername, text='SEND',bg="#cc7722", fg="white",width=30,font="Arial 20 bold")
        self.InitUI()

    def InitUI(self):

        self.usrwnd.grab_set()
        self.usrwnd['bg'] = '#00a86b'
        self.usrwnd.title('PyClient')
        self.usrwnd.geometry('400x300')
        self.usrwnd.resizable(width=False, height=False)
        self.frame.place(relwidth=1, relheight=1)
        self.lbl.pack(pady=10)
        self.usrname.pack(pady=10,padx=20)
        self.lbl2.pack(pady=10)
        self.password.pack(pady=10,padx=20)
        self.ackbtn.pack(pady=10,padx=30)
        self.ackbtn['font']='BOLD'
    def AckUsername(self):
        if (self.usrname.get() != ''):
            m = msg.Message.SendMessage(msg.MR_BROKER, msg.MT_INIT, self.usrname.get()+" "+self.password.get())
            if (m.Header.hactioncode == msg.MT_DECLINE):
                messagebox.showerror('Wrong username or password')
                self.usrname.delete(0, 'end')
                self.password.delete(0, 'end')
            else:
                self.app.username = self.usrname.get()

                self.app.MsgList.insert(1, f"Server: Hello {self.app.username}!")
                for p in m.Data.split('\n'):
                    self.app.MsgList.insert('end', p)
                #if (os.path.exists(path+'history'+self.app.username+'.txt')):
                 #   self.app.MsgList.insert('end', "nice")
                  #  HistoryRead(self.app,self.app.username)
                t = threading.Thread(target=ProcessMessages, args=(self.app,))
                t.start()
                self.usrwnd.grab_release()
                self.usrwnd.destroy()


def ProcessMessages(app):
    while True:
        m = msg.Message.SendMessage(msg.MR_BROKER, msg.MT_REFRESH, str(len(app.ActiveUsers)))
        if m.Header.hactioncode != msg.MT_DECLINE:
            app.RefreshUsers(str(m.Data))
        m = msg.Message.SendMessage(msg.MR_BROKER, msg.MT_GETDATA)
        if m.Header.hactioncode == msg.MT_DATA:
            app.MsgList.insert('end', f"{app.ActiveUsers[m.Header.hfrom]}: {m.Data}")
           # HistoryWrite(app.username,f"{app.ActiveUsers[m.Header.hfrom]}: {m.Data}")
        elif m.Header.hactioncode == msg.MT_EXIT:
            m = msg.Message.SendMessage(msg.MR_BROKER, msg.MT_EXIT)
            app.root.destroy()
        else:
            time.sleep(1)

path="C:\\Users\\User\\Downloads\\MsgSockets\\Debug\\"

def HistoryWrite(username,str):
    file = open(path+'history'+username+'.txt','a',encoding='utf-8')
    file.write(str)
    file.close()
def HistoryRead(app,username):
    file = open(path+'history'+username+'.txt','r',encoding='utf-8')
    bl=True
    while bl:
        str=file.readline()
        if not str:
            bl=False
        else:
            app.MsgList.insert('end',str)

    file.close()
def main():
    app = MainWindow()
    app.UsrList.insert(1, 'All users')
    # msg.Message.SendMessage(msg.MR_BROKER, msg.MT_INIT, 'pyuser')
    UsernameWindow(app)
    app.root.protocol("WM_DELETE_WINDOW", app.on_closing)
    app.root.mainloop()


if __name__ == '__main__':
    main()