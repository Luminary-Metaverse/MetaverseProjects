const express = require('express');
const http = require('http');
const socketIO = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIO(server);

const PORT = 1032;

const otherPlayers = {};
const otherPlayerHands = {};

io.on('connection', (socket) => {
  console.log('A user connected');

  socket.emit('setPlayerId', socket.id);
  socket.broadcast.emit('playerConnected');

  // Menerima event playerMovement dari klien
  socket.on('playerMovement', (data) => {
    socket.broadcast.emit('updatePlayerMovement', data);
  });

  // Menerima event gravityMovement dari klien
  socket.on('gravityMovement', (data) => {
    socket.broadcast.emit('updateGravity', data);
  });

  // Menerima event updateLeftHandMovement dari klien
  socket.on('updateLeftHandMovement', (data) => {
    socket.broadcast.emit('updateLeftHandMovement', data);
  });

  // Menerima event updateRightHandMovement dari klien
  socket.on('updateRightHandMovement', (data) => {
    socket.broadcast.emit('updateRightHandMovement', data);
  });

  // Menanggapi event disconnect dari klien
  socket.on('disconnect', () => {
    console.log('A user disconnected');

    const disconnectedPlayerId = socket.id;

    // Menghapus pemain dari pemain lain
    if (otherPlayers[disconnectedPlayerId]) {
      delete otherPlayers[disconnectedPlayerId];
      socket.broadcast.emit('updateOtherPlayerDisconnected', disconnectedPlayerId);
    }

    // Menghapus tangan pemain (kiri) dari objek tangan pemain lain
    if (otherPlayerHands[disconnectedPlayerId + '_left']) {
      delete otherPlayerHands[disconnectedPlayerId + '_left'];
    }

    // Menghapus tangan pemain (kanan) dari objek tangan pemain lain
    if (otherPlayerHands[disconnectedPlayerId + '_right']) {
      delete otherPlayerHands[disconnectedPlayerId + '_right'];
    }
  });
});

server.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});
