import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./components/Home"; // Putanja do tvog novog Home fajla
import Login from "./components/Login"; // Tvoja login stranica
import Register from "./components/Register"; // Tvoja register stranica
import Room from "./components/Room"; // Ono što si mi malopre pokazao (sa kreriranjem sobe)
import { AuthProvider } from "./context/AuthContext";

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          {/* Glavna stranica (ona sa animacijom) */}
          <Route path="/" element={<Home />} />
          
          {/* Ostale stranice */}
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          
          {/* Stranica gde se bira soba (ona tvoja stara Room komponenta) */}
          <Route path="/game" element={<Room />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;