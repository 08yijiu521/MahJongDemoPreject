using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using GameProtocol;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ConnectServer {



    private Socket socket;
    private int port = 6655;
    private string ip = "120.55.69.92";//192.168.120.76//120.55.69.92
    private byte[] readbuff = new byte[1024];
    private bool isReading = false;
    private List<byte> cache = new List<byte>();
    public Queue<SocketModel> messages = new Queue<SocketModel>();  

    public void OnClickConnect()
    {
        //创建客户端连接对象
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //连接到服务器
        socket.Connect(ip, port);
        //开启异步消息接收 消息到达后会直接写入 缓冲区 readbuff
        socket.BeginReceive(readbuff, 0, 1024, SocketFlags.None, ReceiveCallBack, readbuff);
    }

    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            //获取当前收到的消息长度()
            int length = socket.EndReceive(ar);
            byte[] message = new byte[length];
            Buffer.BlockCopy(readbuff, 0, message, 0, length);
            cache.AddRange(message);
            if (!isReading)
            {
                isReading = true;
                onData();
            }
            //尾递归 再次开启异步消息接收 消息到达后会直接写入 缓冲区 readbuff
            socket.BeginReceive(readbuff, 0, 1024, SocketFlags.None, ReceiveCallBack, readbuff);
        }
        catch (Exception)
        {
            Debug.Log("远程服务器主动断开连接");
            socket.Close();
        }
    }


    //缓存中有数据处理
    void onData()
    {
        //长度解码
        byte[] result = decode(ref cache);

        //长度解码返回空 说明消息体不全，等待下条消息过来补全
        if (result == null)
        {
            isReading = false;
            return;
        }

        SocketModel message = mdecode(result);

        if (message == null)
        {
            isReading = false;
            return;
        }
        //进行消息的处理
        messages.Enqueue(message);
        //尾递归 防止在消息处理过程中 有其他消息到达而没有经过处理
        onData();
    }

    //public void OnClickSend()
    //{
        //write(Protocol.TEST_STRING, 0, 99, input.text);
    //}


    public void write(byte type, int area, int command, object message)
    {
        ByteArray ba = new ByteArray();
        ba.write(type);
        ba.write(area);
        ba.write(command);
        //判断消息体是否为空  不为空则序列化后写入
        if (message != null)
        {
            ba.write(SerializeUtil.encode(message));
        }
        ByteArray arr1 = new ByteArray();
        arr1.write(ba.Length);
        arr1.write(ba.getBuff());
        try
        {
            socket.Send(arr1.getBuff());
        }
        catch (Exception e)
        {
            Debug.Log("网络错误，请重新登录" + e.Message);
        }
    }






    public static byte[] decode(ref List<byte> cache)
    {
        if (cache.Count < 4) return null;

        MemoryStream ms = new MemoryStream(cache.ToArray());//创建内存流对象，并将缓存数据写入进去
        BinaryReader br = new BinaryReader(ms);//二进制读取流
        int length = br.ReadInt32();//从缓存中读取int型消息体长度
        //如果消息体长度 大于缓存中数据长度 说明消息没有读取完 等待下次消息到达后再次处理
        if (length > ms.Length - ms.Position)
        {
            return null;
        }
        //读取正确长度的数据
        byte[] result = br.ReadBytes(length);
        //清空缓存
        cache.Clear();
        //将读取后的剩余数据写入缓存
        cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
        br.Close();
        ms.Close();
        return result;
    }


    public static SocketModel mdecode(byte[] value)
    {
        ByteArray ba = new ByteArray(value);
        SocketModel model = new SocketModel();
        byte type;
        int area;
        int command;
        //从数据中读取 三层协议  读取数据顺序必须和写入顺序保持一致
        ba.read(out type);
        ba.read(out area);
        ba.read(out command);
        model.type = type;
        model.area = area;
        model.command = command;
        //判断读取完协议后 是否还有数据需要读取 是则说明有消息体 进行消息体读取
        if (ba.Readnable)
        {
            byte[] message;
            //将剩余数据全部读取出来
            ba.read(out message, ba.Length - ba.Position);
            //反序列化剩余数据为消息体
            model.message = SerializeUtil.decode(message);
        }
        ba.Close();
        return model;
    }

    public void Close()
    {
        socket.Close();
        socket = null;
    }
}
