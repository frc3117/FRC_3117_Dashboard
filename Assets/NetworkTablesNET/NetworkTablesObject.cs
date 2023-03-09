using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NetworkTablesNET
{
    public enum NetworkTablesVersion
    {
        NT3,
        NT4
    }
    
    public class NetworkTablesObject : MonoBehaviour
    {
        public static NetworkTablesObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("NetworkTables");
                    _instance = obj.AddComponent<NetworkTablesObject>();
                }

                return _instance;
            }
        }
        private static NetworkTablesObject _instance;

        public UnityEvent OnNetworkTablesConnected;
        public UnityEvent OnNetworkTablesDisconnected;
        
        public NetworkTablesVersion Version;

        [Header("Client Settings")] 
        public string ClientName;
        [Space]
        [InspectorName("Hostname")] public string ClientHostname;
        [InspectorName("Port")] public uint ClientPort;

        [Header("Server Settings")] 
        [InspectorName("Port")] public string ServerPort;

        public bool IsStarted { get; protected set; }
        public bool IsClient { get; protected set; }
        public bool IsServer => !IsClient && IsStarted;

        private NetworkTablesInstance _networkTable;

        private Coroutine _ntCoroutine;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                StartClient();
        }

        public void Log(string text)
        {
            print(text);
        }
        
        public void StartClient()
        {
            if (!IsStarted)
            {
                IsStarted = true;
                IsClient = true;

                _networkTable = NetworkTablesInstance.CreateInstance();
                _networkTable.SetServer(ClientHostname, ClientPort);

                if (Version == NetworkTablesVersion.NT3)
                    _networkTable.StartClient3(ClientName);
                else
                    _networkTable.StartClient4(ClientName);

                _ntCoroutine = StartCoroutine(ClientLoop());
            }
        }
        public void StartClientDelay(float delay)
        {
            if (!IsStarted)
            {
                IsStarted = true;
                IsClient = true;

                StartCoroutine(DelayClient(delay));
            }
        }
        private IEnumerator DelayClient(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            StartClient();
        }
        private IEnumerator ClientLoop()
        {
            var wasConnected = false;
            while (IsStarted)
            {
                var connected = _networkTable.IsConnected();
                if (wasConnected != connected)
                {
                    if (connected)
                        OnNetworkTablesConnected?.Invoke();
                    else
                        OnNetworkTablesDisconnected?.Invoke();
                }

                wasConnected = connected;
                
                yield return false;
            }
        }
        
        public void StartServer()
        {
            _ntCoroutine = StartCoroutine(ServerLoop());
        }
        public void StartServerDelay(float delay)
        {
            StartCoroutine(DelayServer(delay));
        }
        private IEnumerator DelayServer(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            StartServer();
        }
        private IEnumerator ServerLoop()
        {
            while (IsStarted)
            {
                yield return false;
            }
        }

        public void Stop()
        {
            if (IsStarted)
            {
                IsStarted = false;

                if (_ntCoroutine != null)
                    StopCoroutine(_ntCoroutine);

                if (_networkTable.IsConnected())
                {
                    _networkTable.StopClient();
                    OnNetworkTablesDisconnected?.Invoke();
                }

                _networkTable?.Dispose();
            }
        }
        
        private void OnApplicationQuit()
        {
            Stop();
        }
    }
}
