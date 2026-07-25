import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

import MapPicker from "./MapPicker";
import { patientRegisterUser } from "../../services/authService";
import type { PatientRegisterRequest } from "../../types/auth";

type PatientRegisterFormData =
  PatientRegisterRequest & {
    confirmPassword: string;
  };

function RegisterPatientForm() {
  const navigate = useNavigate();

  const [selectedLat, setSelectedLat] =
    useState<number>();

  const [selectedLng, setSelectedLng] =
    useState<number>();

  const {
    register,
    handleSubmit,
    setValue,
    getValues,
    formState: { errors }
  } = useForm<PatientRegisterFormData>();

  const onSubmit = async (
    data: PatientRegisterFormData
  ) => {
    try {
      const {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        confirmPassword,
        ...patientData
      } = data;

      console.log(
        "Patient Data:",
        patientData
      );

      await patientRegisterUser(
        patientData
      );

      alert(
        "Registration successful"
      );

      navigate("/patientLogin");
    } catch (error) {
      if (error instanceof Error) {
        alert(error.message);
      }
    }
  };

  const handleLocationSelect = (
    lat: number,
    lng: number
  ) => {
    setSelectedLat(lat);
    setSelectedLng(lng);

    setValue("latitude", lat);
    setValue("longitude", lng);
  };

  return (
    <div className="min-h-screen bg-gray-100 py-10 px-4">
      <div className="max-w-5xl mx-auto bg-white rounded-2xl shadow-xl p-8">

        <h1 className="text-3xl font-bold text-center mb-2">
          Patient Registration
        </h1>

        <p className="text-center text-gray-500 mb-8">
          Create your patient account
        </p>

        <form
          onSubmit={handleSubmit(
            onSubmit
          )}
          className="space-y-8"
        >
          <div className="grid grid-cols-1 md:grid-cols-2 gap-5">

            {/* Full Name */}
            <div>
              <label className="block mb-2 font-medium">
                Full Name
              </label>

              <input
                type="text"
                {...register(
                  "fullName",
                  {
                    required:
                      "Full name is required"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {
                  errors.fullName
                    ?.message
                }
              </p>
            </div>

            {/* Email */}
            <div>
              <label className="block mb-2 font-medium">
                Email
              </label>

              <input
                type="email"
                {...register(
                  "email",
                  {
                    required:
                      "Email is required"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {
                  errors.email
                    ?.message
                }
              </p>
            </div>

            {/* Phone Number */}
            <div>
              <label className="block mb-2 font-medium">
                Phone Number
              </label>

              <input
                type="text"
                {...register(
                  "phoneNumber",
                  {
                    required:
                      "Phone number is required"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {
                  errors.phoneNumber
                    ?.message
                }
              </p>
            </div>

            {/* Date of Birth */}
            <div>
              <label className="block mb-2 font-medium">
                Date of Birth
              </label>

              <input
                type="date"
                {...register(
                  "dateOfBirth",
                  {
                    required:
                      "Date of birth is required"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {
                  errors.dateOfBirth
                    ?.message
                }
              </p>
            </div>

            {/* Gender */}
            <div>
              <label className="block mb-2 font-medium">
                Gender
              </label>

              <select
                {...register(
                  "gender",
                  {
                    valueAsNumber: true,
                    required:
                      "Gender is required"
                  }
                )}
                className="w-full border rounded-lg p-3"
              >
                <option value="">
                  Select Gender
                </option>

                <option value={0}>
                  Male
                </option>

                <option value={1}>
                  Female
                </option>

                <option value={2}>
                  Other
                </option>
              </select>

              <p className="text-red-500 text-sm">
                {
                  errors.gender
                    ?.message
                }
              </p>
            </div>

            {/* Password */}
            <div>
              <label className="block mb-2 font-medium">
                Password
              </label>

              <input
                type="password"
                {...register(
                  "password",
                  {
                    required:
                      "Password is required"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {
                  errors.password
                    ?.message
                }
              </p>
            </div>

            {/* Confirm Password */}
            <div className="md:col-span-2">
              <label className="block mb-2 font-medium">
                Confirm Password
              </label>

              <input
                type="password"
                {...register(
                  "confirmPassword",
                  {
                    required:
                      "Confirm password is required",
                    validate: (
                      value
                    ) =>
                      value ===
                      getValues(
                        "password"
                      )
                        ? true
                        : "Passwords do not match"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {
                  errors
                    .confirmPassword
                    ?.message
                }
              </p>
            </div>

            {/* Address */}
            <div className="md:col-span-2">
              <label className="block mb-2 font-medium">
                Address
              </label>

              <input
                type="text"
                {...register(
                  "address",
                  {
                    required:
                      "Address is required"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {
                  errors.address
                    ?.message
                }
              </p>
            </div>

          </div>

          {/* Map */}
          <div>
            <h2 className="font-semibold text-lg mb-3">
              Select Your Location
            </h2>

            <MapPicker
              onLocationSelect={
                handleLocationSelect
              }
            />
          </div>

          {/* Coordinates */}
          <div className="grid grid-cols-2 gap-5">

            <div>
              <label className="block mb-2 font-medium">
                Latitude
              </label>

              <input
                readOnly
                value={
                  selectedLat ?? ""
                }
                className="w-full border rounded-lg p-3 bg-gray-100"
              />
            </div>

            <div>
              <label className="block mb-2 font-medium">
                Longitude
              </label>

              <input
                readOnly
                value={
                  selectedLng ?? ""
                }
                className="w-full border rounded-lg p-3 bg-gray-100"
              />
            </div>

          </div>

          <button
            type="submit"
            className="w-full bg-green-600 text-white py-3 rounded-lg font-semibold hover:bg-green-700"
          >
            Register Patient
          </button>

        </form>
      </div>
    </div>
  );
}

export default RegisterPatientForm;