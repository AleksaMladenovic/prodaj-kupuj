import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import type { HubConnection } from '@microsoft/signalr';

interface LobbyProps {
    roomId: string;
    username: string;
}

interface Player {
    connectionId: string;
    username: string;
    isHost: boolean;
}

const Lobby = ({ roomId, username }: LobbyProps) => {
    
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const [players, setPlayers] = useState([]);
    const [chatMessages, setChatMessages] = useState([]); // Bonus: za chat

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl("https://localhost:7277/gamehub")
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    console.log('Povezan!');
                    
                    // Pozovi JoinRoom metodu na backendu
                    connection.invoke("JoinRoom", roomId, username);

                    // Slušaj za ažuriranja liste igrača
                    connection.on("PlayerListUpdated", (updatedPlayers) => {
                        setPlayers(updatedPlayers);
                    });
                    
                    // Slušaj za greške
                    connection.on("Error", (message) => {
                        alert(message);
                        // Vrati korisnika nazad na početni ekran
                        window.location.reload(); 
                    });
                })
                .catch(e => console.error('Greška pri povezivanju: ', e));
        }
    }, [connection, roomId, username]);

    return (
        <div>
            <h1>Lobi (ID: {roomId})</h1>
            <h2>Igrači:</h2>
            <ul>
                {players.map((player: Player) => (
                    <li key={player.connectionId}>{player.username} {player.isHost ? '(Host)' : ''}</li>
                ))}
            </ul>
        </div>
    );
};

export default Lobby;