import type { PatientRegisterRequest,PatientLoginRequest ,DoctorLoginRequest, DoctorRegisterRequest, Specialization } from "../types/auth";

//For registering a new user
export async function patientRegisterUser(
  data: PatientRegisterRequest
) {
  const response = await fetch(
    "https://localhost:7060/api/AuthPatientRegistration/register-patient",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    }
  );

  if (!response.ok) {
    throw new Error(await response.text());
  }

  return response.json();
}

//for registering a new doctor user
export async function doctorRegisterUser(
  data: DoctorRegisterRequest
) {
  const response = await fetch(
    "https://localhost:7060/api/AuthDoctorRegistration/register-doctor",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    }
  );

  if (!response.ok) {
    throw new Error(await response.text());
  }

  return response.json();
}

//For logging in an existing user

export async function PatientLoginUser(
  data: PatientLoginRequest
) {
  const response = await fetch(
    "https://localhost:7060/api/Auth/login",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    }
  );

  const result = await response.json();

  if (!response.ok) {
    throw new Error(result.message);
  }

  return result;
}
//For logging in an existing doctor user
export async function DoctorLoginUser(
  data: DoctorLoginRequest
) {
  const response = await fetch(
    "https://localhost:7060/api/AuthDoctorRegistration/doctorLogin",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    }
  );
 const result = await response.json();
  if (!response.ok) {
    throw new Error(result.message);
  }

  return result;
}

export const getSpecializations = async (): Promise<Specialization[]> => {
  const response = await fetch("https://localhost:7060/api/Specializations/getAllSpecialization");

  if (!response.ok) {
    throw new Error("Failed to fetch specializations");
  }

  const data = await response.json();

  if (Array.isArray(data)) {
    return data;
  }

  if (Array.isArray(data?.data)) {
    return data.data;
  }

  if (Array.isArray(data?.specializations)) {
    return data.specializations;
  }

  return [];
};

