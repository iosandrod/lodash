import socketio from 'socket.io-client'
import io from 'socket.io-client'//
async function main() {
    console.log(' main is run')//
    // let ioClient = socketio('http://localhost:3000', {
    //     timeout: 2000
    // })//
    // //@ts-ignore
    // ioClient.on("connect", async (so) => {
    //     console.log('some one is connect')//
    // })
    // ioClient.on('message', () => {
    //     console.log(' received message')//
    // })
    const socketChat = io('http://localhost:3000/chat');
    socketChat.on('connect', () => {
        console.log('Connected to /chat');
    })//
    // 发送消息到服务器
    // setInterval(() => {
    //     socketChat.emit('message', 'Hello from chat');
    // }, 1000);//
    // 接收服务器返回的消息//
    socketChat.on('message', (msg: string) => {
        console.log('Received in chat:', msg);
    });
    socketChat.emit('message', 'Hello from chat');
}

main()//