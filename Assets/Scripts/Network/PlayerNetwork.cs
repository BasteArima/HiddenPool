using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _runSpeed = 6f;

    [SerializeField] private TMP_Text _randomNumberText;
    [SerializeField] private TMP_Text customDataText;

    [SerializeField] private Transform _spawnedObjectPrefab;

    private Transform _spawnedObjectTransform;
    
    // Можно передавать только value type
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    // Передача структуры
    private NetworkVariable<MyCustomData> customData = new NetworkVariable<MyCustomData>(
        new MyCustomData { score = 1, message = "test" }, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int score;
        public string message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref score);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        _randomNumberText.text = $"Player {OwnerClientId}:<br>randomNumber {randomNumber.Value}";
        customDataText.text = "";

        randomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            _randomNumberText.text = $"Player {OwnerClientId}:<br>randomNumber {randomNumber.Value}";
            Debug.Log($"Player {OwnerClientId}: randomNumber {randomNumber.Value}");
        };

        customData.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            customDataText.text =
                $"customData score {newValue.score}<br>message {newValue.message}";
            Debug.Log($"Player {OwnerClientId}: customData score {newValue.score}, message {newValue.message}");
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log($"Test {OwnerClientId}");
        }

        if (!IsOwner) return;
        TestButtons();
        MovePlayer();
    }

    private void TestButtons()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = Random.Range(0, 100);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            customData.Value = new MyCustomData
                { score = Random.Range(0, 100), message = $"test {Random.Range(0, 100)}" };
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TestServerRpc(randomNumber.Value);
            TestTwoServerRpc(new ServerRpcParams());
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            TestClientRpc();
            TestTwoClientRpc(new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Только сервер может спавнить NetworkObjects
            // Если клиент попытается заспавнить, спавн будет только на клиенте, но не у других клиентов и сервера
            // Т.е. если мы хотим, чтобы клиент вызвал спавн, надо чтобы он ЗАПРОСИЛ у СЕРВЕРА спавн
            if (!IsServer) return;
            
            _spawnedObjectTransform = Instantiate(_spawnedObjectPrefab);
            _spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnObjectServerRpc();
        }
        
        // Вроде нормально работает и удаление и деспавн, только деспавн не уничтожает предмет кажется, пока не понял зачем он и в чём разница
        if (Input.GetKeyDown(KeyCode.J))
        {
            //_spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(_spawnedObjectTransform.gameObject);
        }
    }

    // Метод можно вызвать с любого клиента, но сработает он только на сервере
    // Можно через него передавать данные серверу и вызывать методы только сервера
    // Обязательно название метода должно заканчиваться на ServerRpc
    // Нельзя передавать ссылочные типы, кроме string
    [ServerRpc]
    private void TestServerRpc(int data)
    {
        Debug.Log($"TestServerRpc {OwnerClientId}, data {data}");
    }

    [ServerRpc]
    private void TestTwoServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log($"TestServerRpc {OwnerClientId}, serverRpcParams {serverRpcParams.Receive.SenderClientId}");
    }
    
    [ServerRpc]
    private void SpawnObjectServerRpc()
    {
        var spawnedObject = Instantiate(_spawnedObjectPrefab);
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);
    }

    // Вызывать может только сервер, но сработает на ВСЕХ КЛИЕНТАХ
    // Почему-то если поднимать Server именно без Host, то не срабатывает тут, видимо объект не считается существующим
    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log($"Client rpc test!");
    }

    // При вызове можно выбрать определенные ID игроков, которые вызовут у себя этот метод
    [ClientRpc]
    private void TestTwoClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log($"Client rpc test! ");
    }

    private void MovePlayer()
    {
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _moveSpeed;

        transform.position += moveDir * speed * Time.deltaTime;
    }
}