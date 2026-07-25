import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

import MapPicker from "./MapPicker";
import { doctorRegisterUser, getSpecializations } from "../../services/authService";
import type { DoctorRegisterRequest, Specialization } from "../../types/auth";

function RegisterDoctorForm() {
  const navigate = useNavigate();

type DoctorRegisterFormData =
  DoctorRegisterRequest & {
    confirmPassword: string
  };

const [specializations, setSpecializations] =
  useState<Specialization[]>([]);

useEffect(() => {
  const fetchSpecializations = async () => {
    try {
      const data =
        await getSpecializations();

      setSpecializations(data);
    } catch (error) {
      console.error(
        "Failed to load specializations",
        error
      );
    }
  };

  fetchSpecializations();
}, []);

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
  } = useForm<DoctorRegisterFormData>();


  const onSubmit = async (
    data: DoctorRegisterFormData
  ) => {
    try {
      const {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        confirmPassword,
        ...doctorData
      } = data;

      console.log("Doctor data:", doctorData);
      await doctorRegisterUser(doctorData);
      alert("Registration successful");

      navigate("/doctorLogin");
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

        <h1 className="text-3xl font-bold text-center mb-8">
          Doctor Registration
        </h1>
  <p className="text-center text-gray-500 mb-8">
          Create your doctor account
        </p>
        <form
          onSubmit={handleSubmit(onSubmit)}
          className="space-y-8"
        >

          <div className="grid grid-cols-1 md:grid-cols-2 gap-5">

            <div>
              <label>Full Name</label>

              <input
                type="text"
                {...register("fullName", {
                  required: "Full name required"
                })}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {errors.fullName?.message}
              </p>
            </div>

            <div>
              <label>Email</label>

              <input
                type="email"
                {...register("email", {
                  required: "Email required"
                })}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {errors.email?.message}
              </p>
            </div>

            <div>
              <label>Phone Number</label>

              <input
                type="text"
                {...register("phoneNumber", {
                  required: "Phone number required"
                })}
                className="w-full border rounded-lg p-3"
              />
            </div>

            <div>
              <label>Degree</label>

              <input
                type="text"
                {...register("degree", {
                  required: "Degree required"
                })}
                className="w-full border rounded-lg p-3"
              />
            </div>

            <div>
              <label>Experience (Years)</label>

              <input
                type="number"
                {...register("experience", {
                  valueAsNumber: true,
                  required: "Experience required"
                })}
                className="w-full border rounded-lg p-3"
              />
            </div>

            <div>
              <label>Registration ID</label>

              <input
                type="text"
                {...register("registrationId", {
                  required:
                    "Registration ID required"
                })}
                className="w-full border rounded-lg p-3"
              />
            </div>

            <div>
              <label>Specialization</label>

           <select
  {...register("specializationId", {
    valueAsNumber: true,
    required: "Specialization required"
  })}
  className="w-full border rounded-lg p-3"
>
  <option value="">
    Select Specialization
  </option>

  {specializations.map(
    (specialization) => (
      <option
        key={specialization.specializationId}
        value={specialization.specializationId}
      >
        {specialization.name}
      </option>
    )
  )}
</select>
            </div>

            <div>
              <label>Password</label>

              <input
                type="password"
                {...register("password", {
                  required: "Password required"
                })}
                className="w-full border rounded-lg p-3"
              />
            </div>

            <div className="md:col-span-2">
              <label>Confirm Password</label>

              <input
                type="password"
                {...register(
                  "confirmPassword",
                  {
                    required:
                      "Confirm password required",
                   validate: (value) =>
  value === getValues("password")
    ? true
    : "Passwords do not match"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />

              <p className="text-red-500 text-sm">
                {
                  errors.confirmPassword
                    ?.message
                }
              </p>
            </div>

            <div className="md:col-span-2">
              <label>Clinic Address</label>

              <input
                type="text"
                {...register(
                  "clinicAddress",
                  {
                    required:
                      "Clinic address required"
                  }
                )}
                className="w-full border rounded-lg p-3"
              />
            </div>
          </div>

          <div>
            <h2 className="font-semibold text-lg mb-3">
              Select Clinic Location
            </h2>

            <MapPicker
              onLocationSelect={
                handleLocationSelect
              }
            />
          </div>

          <div className="grid grid-cols-2 gap-5">

            <div>
              <label>Latitude</label>

              <input
                readOnly
                value={selectedLat ?? ""}
                className="w-full border rounded-lg p-3 bg-gray-100"
              />
            </div>

            <div>
              <label>Longitude</label>

              <input
                readOnly
                value={selectedLng ?? ""}
                className="w-full border rounded-lg p-3 bg-gray-100"
              />
            </div>

          </div>

          <button
            type="submit"
            className="w-full bg-blue-600 text-white py-3 rounded-lg font-semibold hover:bg-blue-700"
          >
            Register Doctor
          </button>

        </form>
      </div>
    </div>
  );
}

export default RegisterDoctorForm;