using UnityEngine;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Connection : MonoBehaviour
{
    [SerializeField] private List<UnityEventString> _commandDictionary = new List<UnityEventString>();

    private ClientWebSocket _webSocket;
    private CancellationTokenSource _cancellationTokenSource;

    private async void Start()
    {
        _webSocket = new ClientWebSocket();
        _cancellationTokenSource = new CancellationTokenSource();

        var uri = new Uri("wss://irc-ws.chat.twitch.tv:443");

        await _webSocket.ConnectAsync(uri, _cancellationTokenSource.Token);

        await SendWebSocketMessage("PASS " + "oauth:pv43i8tpibwbs8aflxwgk7ombr2k3d");
        await SendWebSocketMessage("NICK " + "charsdev");
        await SendWebSocketMessage("JOIN #" + "charsdev");

        ReceiveMessage();
    }

    private async Task SendWebSocketMessage(string command)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(command);
        var arraySegment = new ArraySegment<byte>(buffer);
        await _webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
    }

    private async void ReceiveMessage()
    {
        try
        {
            var buffer = new byte[4096];
            var receiverBuffer = new ArraySegment<byte>(buffer);
            var stringBuilder = new StringBuilder();

            while (_webSocket.State == WebSocketState.Open) 
            {
                stringBuilder.Clear();

                WebSocketReceiveResult receiveResult;

                do
                {
                    receiveResult = await _webSocket.ReceiveAsync(receiverBuffer, _cancellationTokenSource.Token);
                    var strResult = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    stringBuilder.Append(strResult);
                } while (!receiveResult.EndOfMessage);

                string message = stringBuilder.ToString();

                if (string.IsNullOrEmpty(message))
                {
                    return;
                }

                ProcessChatMessage(message);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        finally 
        { 
            _webSocket?.Dispose();
        }
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
        _webSocket?.Dispose();
    }

    private void ProcessChatMessage(string message)
    {
        string[] splitMessage = message.Split(':');

        if (splitMessage.Length >= 3)
        {
            string msg = splitMessage[2].Trim();

            print(msg);
            foreach (var command in _commandDictionary)
            {
                if (msg.Equals(command.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    command.Event.Invoke();
                }
            }
        }
    }
}
