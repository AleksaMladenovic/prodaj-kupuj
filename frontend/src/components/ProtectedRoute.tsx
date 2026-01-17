import { JSX, useContext, useEffect } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import {  } from "firebase/auth";

interface ProtectedRouteProps {
  children: JSX.Element;
}

const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
  const { user, loading } = useAuth();

  // Čekaj dok se Firebase auth inicijalizuje
  if (loading) {
    return <div className="flex items-center justify-center min-h-screen bg-neutral-950">
      <div className="animate-spin rounded-full h-16 w-16 border-t-4 border-red-600 border-opacity-50"></div>
    </div>
  }

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  // Provera da li je email verifikovan direktno iz Firebase user objekta
  if (!user.emailVerified) {
    return <Navigate to="/verify-email" replace />;
  }

  return children;
};

export default ProtectedRoute;
