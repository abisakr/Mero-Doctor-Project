import "tailwindcss";
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import Home from './pages/Home'
import About from './pages/About'
import Weather from './pages/Weather'
import RegisterForm from "./components/forms/RegisterDoctorForm";
import DoctorLoginForm from "./components/forms/DoctorLoginForm";
import PatientLoginForm from "./components/forms/PatientLoginForm";
import RegisterPatientForm from "./components/forms/RegisterPatientForm";

function App() {

  return (
  <BrowserRouter>
      <nav>
        <Link to="/">Home</Link> | <Link to="/about">About</Link> | <Link to="/weather">Weather</Link> | <Link to="/doctorlogin">Doctor Login</Link> | <Link to="/patientlogin">Patient Login</Link>
      </nav>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/weather" element={<Weather />} />
        <Route path="/doctorlogin" element={<DoctorLoginForm />} />
        <Route path="/patientlogin" element={<PatientLoginForm />} />
        <Route path="/register-doctor" element={<RegisterForm />} />
        <Route path="/register-patient" element={<RegisterPatientForm />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
