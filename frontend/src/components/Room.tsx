import React, { useState } from 'react';
import axios from 'axios';
import Lobby from './Lobby';

const API_URL = "https://localhost:7277"; // Tvoj backend URL

function Room() {
    // Stanje koje kontroliše šta se prikazuje na ekranu
    const [roomId, setRoomId] = useState<string | null>(null);
    const [username, setUsername] = useState('');

    // Stanje za input polja
    const [joinRoomIdInput, setJoinRoomIdInput] = useState('');
    const [usernameInput, setUsernameInput] = useState('');

    // --- Akcije korisnika ---

    const handleCreateRoom = async () => {
        if (!usernameInput.trim()) {
            alert("Molimo unesite korisničko ime.");
            return;
        }
        try {
            const response = await axios.post(`${API_URL}/api/Rooms/create`);
            setUsername(usernameInput);
            setRoomId(response.data.roomId); // Ovo će prebaciti prikaz na Lobby komponentu
        } catch (error) {
            console.error("Greška pri kreiranju sobe:", error);
            alert("Nije moguće kreirati sobu.");
        }
    };

    const handleJoinRoom = () => {
        if (!usernameInput.trim() || !joinRoomIdInput.trim()) {
            alert("Molimo unesite korisničko ime i ID sobe.");
            return;
        }
        setUsername(usernameInput);
        setRoomId(joinRoomIdInput); // Prebacujemo prikaz na Lobby
    };


    // --- Logika za renderovanje ---

    // Ako nemamo roomId, prikaži početni ekran
    if (!roomId) {
        return (
            <div>
                <h1>Dobrodošli u Impostor Game!</h1>
                <input
                    type="text"
                    placeholder="Unesite vaše ime"
                    value={usernameInput}
                    onChange={(e) => setUsernameInput(e.target.value)}
                />
                <hr />
                <div>
                    <button onClick={handleCreateRoom}>Napravi novu sobu</button>
                </div>
                <hr />
                <div>
                    <input
                        type="text"
                        placeholder="Unesite ID sobe"
                        value={joinRoomIdInput}
                        onChange={(e) => setJoinRoomIdInput(e.target.value.toUpperCase())}
                    />
                    <button onClick={handleJoinRoom}>Pridruži se sobi</button>
                </div>
            </div>
        );
    }
    
    // Ako imamo roomId, prikaži lobi
    return <Lobby roomId={roomId} username={username} />;
}

export default Room;