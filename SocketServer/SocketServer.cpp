// SereginIntegratedSys.cpp : ���� ���� �������� ������� "main". ����� ���������� � ������������� ���������� ���������.
//

#include "pch.h"
#include "framework.h"
#include "SocketServer.h"
#include "Message.h"
#include "Session.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


CWinApp theApp;
using namespace std;

void LaunchClient()
{
    STARTUPINFO si = { sizeof(si) };
    PROCESS_INFORMATION pi;
    CreateProcess(NULL, (LPSTR)"C:\\Users\\User\\AppData\\Local\\Microsoft\\WindowsApps\\python3.9.exe C:\\Users\\User\\Downloads\\MsgSockets\\SaveMessage\\main.py", NULL, NULL, TRUE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &pi);
    CloseHandle(pi.hThread);
    CloseHandle(pi.hProcess);
}

int maxID = MR_USER;
map<int, shared_ptr<Session>> sessions;
CSocket HistorianSocket;
CCriticalSection cs;

string GetActiveUsers()
{
    string NamesAndIds = "";
    for (auto& session : sessions)
    {
        NamesAndIds = NamesAndIds + to_string(session.second->id) + " " + session.second->GetName() + " ";
    }
    return NamesAndIds;
}

void CheckIfUserIsInactive()
{
    int timespan = 100000;
    while (true)
    {
        if (sessions.size() > 0)
        {
            for (auto& session : sessions)
            {
                if (chrono::duration_cast<chrono::milliseconds>(chrono::steady_clock::now()
                    - session.second->GetLastSeen()).count() > timespan)
                {
                    cout<<"ok" << endl;
                    cs.Lock();
                    for (auto& [id,sessionn] : sessions)
                    {
                        if (id != session.first) {
                            Message m(sessionn->id, MR_BROKER, MT_DATA,"user "+session.second->GetName() + " disconnected");
                            sessionn->MessageAdd(m);
                        }
                    }
                    cs.Unlock();
                    cout << "Session " + to_string(session.first) + " deleted" << endl;
                    sessions.erase(session.first);
                    break;
                }
            }
        }
        cs.Lock();
        cout << "CheckIfUserIsInactive() sleeping" << endl;
        cout << GetActiveUsers() << endl;
        cs.Unlock();
        Sleep(timespan);
    }
}

void ClientProcessing(SOCKET hSock)
{
    CSocket s;
    s.Attach(hSock);
    Message m;
    int code = m.Receive(s);
    switch (code)
    {
    case MT_INIT:
    {
        bool isDeclined = true;
        Message::Send(HistorianSocket, MR_HISTORIAN, MR_BROKER, MT_INIT, m.GetData());
        Message mh;
        int hcode = mh.Receive(HistorianSocket);
        if (hcode == MT_CONFIRM)
            isDeclined = false;
        /*for (auto& [id, iSession] : sessions)
        {
            if (iSession->GetName() == m.GetData())
            {
                Message::Send(s, 0, MR_BROKER, MT_DECLINE);
                isDeclined = true;
                cs.Lock();
                cout << "error" << endl;
                cs.Unlock();
            }
        }*/
        if (!isDeclined)
        {
            stringstream IdAndName(mh.GetData());
            int id;
            string uname;
            IdAndName >> id;
            IdAndName >> uname;
            auto session = make_shared<Session>(int(id), uname);
            sessions[session->id] = session;
            Message::Send(HistorianSocket, MR_HISTORIAN, session->id, MT_REFRESH);
            int hcode = mh.Receive(HistorianSocket);
            Message::Send(s, session->id, MR_BROKER, MT_INIT, mh.GetData());
            cs.Lock();
            cout << session->id << " (" << session->GetName() << ") connected" << endl;
            cs.Unlock();
            session->SetLastSeen();
        }
        else
        {
            Message::Send(s, MR_USER, MR_BROKER, MT_DECLINE);
            cs.Lock();
            cout << "error" << endl;
            cs.Unlock();
        }
        break;
    }
    case MT_EXIT:
    {
        sessions.erase(m.GetFrom());
        //Message::Send(s, m.GetFrom(), MR_BROKER, MT_CONFIRM);
        cout << m.GetFrom() << " exited" << endl;
        break;
    }
    case MT_GETDATA:
    {
        auto iSession = sessions.find(m.GetFrom());
        if (iSession != sessions.end())
        {
            iSession->second->MessageSend(s);
            iSession->second->SetLastSeen();
        }
        break;
    }
    case MT_REFRESH:
    {
        if (stoi(m.GetData()) != int(sessions.size()))
        {
            if (m.GetFrom() == MR_REST)
            {
                Message::Send(s, MR_REST, MR_BROKER, MT_REFRESH, (GetActiveUsers() + "-1"));
                cs.Lock();
                cout << "MR_REST refreshed" << endl;
                cs.Unlock();
            }
            auto iSession = sessions.find(m.GetFrom());
            if (iSession != sessions.end())
            {
                Message::Send(s, iSession->second->id, MR_BROKER, MT_REFRESH, (GetActiveUsers() + "-1"));
                cs.Lock();
                cout << iSession->second->id << " refreshed" << endl;
                cs.Unlock();
            }
        }
        else
            Message::Send(s, m.GetFrom(), MR_BROKER, MT_DECLINE);
        break;
    }
    default:
    {
        auto iSessionFrom = sessions.find(m.GetFrom());
        if (iSessionFrom != sessions.end())
        {
            iSessionFrom->second->SetLastSeen();
            auto iSessionTo = sessions.find(m.GetAddr());
            if (iSessionTo != sessions.end())
            {
                iSessionTo->second->MessageAdd(m);
                Message::Send(HistorianSocket, m.GetAddr(), m.GetFrom(), MT_DATA, m.GetData());
            }
            else if (m.GetAddr() == MR_ALL)
            {
                for (auto& [id, session] : sessions)
                {
                    if (id != m.GetFrom())
                        session->MessageAdd(m);
                }
                Message::Send(HistorianSocket, MR_ALL, m.GetFrom(), MT_DATA, m.GetData());
            }
        }
        break;
    }
    }
}

void Server()
{
    AfxSocketInit();
    CSocket Server;
    Server.Create(12345);
    printf("Server started\n");
    thread t1(CheckIfUserIsInactive);
    t1.detach();
    
    while (true)
    {
        LaunchClient();
        if (!Server.Listen())
            break;
        Server.Accept(HistorianSocket);
        Message m;
        int code = m.Receive(HistorianSocket);
        if (code == MT_INIT && m.GetFrom() == MR_HISTORIAN)
        {
            cout << "Historian connected!" << endl;
            Message::Send(HistorianSocket, MR_HISTORIAN, MR_BROKER, MT_INIT, "start");
            break;
        }
        else
        {
            Message::Send(HistorianSocket, 0, MR_BROKER, MT_DECLINE);
            HistorianSocket.Detach();
        }
    }
    while (true)
    {
        if (!Server.Listen())
            break;
        CSocket s;
        Server.Accept(s);
        thread t(ClientProcessing, s.Detach());
        t.detach();
    }

    Server.Close();
    printf("Server stoped");
}



int main()
{
    int nRetCode = 0;

    HMODULE hModule = ::GetModuleHandle(nullptr);

    if (hModule != nullptr)
    {
        // ���������������� MFC, � ����� ������ � ��������� �� ������� ��� ����
        if (!AfxWinInit(hModule, nullptr, ::GetCommandLine(), 0))
        {
            // TODO: �������� ���� ��� ��� ����������.
            wprintf(L"����������� ������: ���� ��� ������������� MFC\n");
            nRetCode = 1;
        }
        else
        {
            Server();
            // TODO: �������� ���� ��� ��� ����������.
        }
    }
    else
    {
        // TODO: �������� ��� ������ � ������������ � �������������
        wprintf(L"����������� ������: ���� GetModuleHandle\n");
        nRetCode = 1;
    }

    return nRetCode;
}