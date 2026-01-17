import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { motion } from "framer-motion";

const Home = () => {
    const navigate = useNavigate();
    const { user, loggedUser, logout } = useAuth();

    const handleLoginClick = () => navigate("/login");
    const handleRegisterClick = () => navigate("/register");
    const handleLogoutClick = () => logout();

    return (
        <div className="min-h-screen bg-neutral-950 flex flex-col items-center justify-center text-white overflow-hidden font-sans">
            {/* Kontejner za animaciju naslova */}
            <motion.div 
                initial={{ opacity: 0, y: 20, scale: 0.9 }}
                animate={{ opacity: 1, y: 0, scale: 1 }}
                transition={{ duration: 1.2, ease: "easeOut" }}
                className="text-center"
            >
                <h1 className="text-6xl md:text-8xl font-black tracking-tighter mb-4 bg-gradient-to-b from-white to-gray-500 bg-clip-text text-transparent italic">
                    IMPOSTOR GAME
                </h1>
                
                <motion.p 
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 0.6 }}
                    transition={{ delay: 0.8, duration: 1 }}
                    className="text-lg tracking-widest uppercase mb-12"
                >
                    Find the Impostor or be the Impostor!
                </motion.p>
            </motion.div>

            {/* Dugmići sa animacijom */}
            <motion.div 
                initial={{ opacity: 0, y: 30 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 1.2, duration: 0.8 }}
                className="flex flex-col gap-4 w-64"
            >
                {loggedUser() ? (
                    <div className="flex flex-col items-center gap-6">
                        <div className="bg-white/5 border border-white/10 px-6 py-3 rounded-2xl backdrop-blur-sm">
                            <p className="text-gray-400 text-sm">Prijavljeni ste kao:</p>
                            <p className="font-medium text-white">{user?.email}</p>
                        </div>
                        
                        <button 
                            className="w-full py-4 bg-white text-black font-bold rounded-xl hover:bg-gray-200 transition-all active:scale-95 shadow-[0_0_20px_rgba(255,255,255,0.2)]"
                            onClick={() => navigate("/game")} // Ovde ideš na stranicu za kreiranje sobe
                        >
                            IGRAJ ODMAH
                        </button>

                        <button 
                            className="text-gray-500 hover:text-red-400 transition-colors text-sm font-semibold tracking-wide uppercase" 
                            onClick={handleLogoutClick}
                        >
                            Odjavi se
                        </button>
                    </div>
                ) : (
                    <div className="flex flex-col gap-4">
                        <button 
                            className="w-full py-4 bg-white text-black font-bold rounded-xl hover:bg-gray-200 transition-all active:scale-95 shadow-[0_0_20px_rgba(255,255,255,0.2)]"
                            onClick={handleLoginClick}
                        >
                            ULOGUJ SE
                        </button>
                        
                        <button 
                            className="w-full py-4 bg-transparent border-2 border-white/20 text-white font-bold rounded-xl hover:bg-white/10 hover:border-white/40 transition-all active:scale-95"
                            onClick={handleRegisterClick}
                        >
                            REGISTRACIJA
                        </button>
                    </div>
                )}
            </motion.div>

            {/* Pozadinski detalj (Suptilni sjaj) */}
            <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[500px] h-[500px] bg-red-900/10 blur-[120px] rounded-full -z-10" />
        </div>
    );
}

export default Home;