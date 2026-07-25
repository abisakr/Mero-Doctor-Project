export type PatientLoginRequest = {
  email: string;
  password: string;
};

export type DoctorLoginRequest = {
  registrationId: string;
  password: string;
};

export type PatientRegisterRequest = {
  fullName: string;
  email: string;
  phoneNumber: string;
  dateOfBirth: string;
  gender: number;
  address: string;
  latitude?: number;
  longitude?: number;
  password: string;
};

export type DoctorRegisterRequest = {
  fullName: string;
  email: string;
  phoneNumber: string;
  degree: string;
  experience: number;
  registrationId: string;
  clinicAddress: string;
  specializationId: number;
  latitude?: number;
  longitude?: number;
  password: string;
};

export type Specialization = {
  specializationId: number;
  name: string;
};