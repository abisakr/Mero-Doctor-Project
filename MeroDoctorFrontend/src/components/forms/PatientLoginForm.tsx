import { useForm } from "react-hook-form";
import type { PatientLoginRequest } from "../../types/auth";
import { PatientLoginUser } from "../../services/authService";
import { useNavigate } from "react-router-dom";

function LoginForm() {
  const {
    register,
    handleSubmit,
    formState: { errors }
  } = useForm<PatientLoginRequest>();
 const navigate = useNavigate();

 const onSubmit = async (data: PatientLoginRequest) => {
  try {
    const response = await PatientLoginUser(data);

    alert(response.message);
    navigate("/");
  } catch (error) {
    if (error instanceof Error) {
      alert(error.message);
    } else {
      alert("Something went wrong.");
    }
  }
};

const handleGoogleLogin = () => {
  window.location.href =
    "https://localhost:5001/api/auth/google-login";
};

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center px-4">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-xl p-8">
        <h1 className="text-3xl font-bold text-center text-gray-800 mb-2">
          Welcome Back
        </h1>

        <p className="text-center text-gray-500 mb-8">
          Sign in to your account
        </p>

        <form
          onSubmit={handleSubmit(onSubmit)}
          className="space-y-5"
        >
          <div>
            <label className="block mb-2 text-sm font-medium text-gray-700">
              Email
            </label>

            <input
              type="text"
              placeholder="Enter your email"
              {...register("email", {
                required: "Email is required",
                pattern: {
                  value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
                  message: "Invalid email address"
                }
              })}
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />

            {errors.email && (
              <p className="mt-1 text-sm text-red-500">
                {errors.email.message}
              </p>
            )}
          </div>

          <div>
            <label className="block mb-2 text-sm font-medium text-gray-700">
              Password
            </label>

            <input
              type="password"
              placeholder="Enter your password"
              {...register("password", {
                required: "Password is required"
              })}
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />

            {errors.password && (
              <p className="mt-1 text-sm text-red-500">
                {errors.password.message}
              </p>
            )}
          </div>

        <button
  type="submit"
  className="w-full bg-blue-600 text-white py-3 rounded-lg font-semibold hover:bg-blue-700 transition"
>
  Login
</button>
        </form>

<div className="flex items-center my-4">
  <div className="flex grow border-t border-gray-300"></div>
  <span className="mx-4 text-sm text-gray-500">OR</span>
  <div className="flex grow border-t border-gray-300"></div>
</div>

<button
  type="button"
  onClick={handleGoogleLogin}
  className="w-full flex items-center justify-center gap-3 border border-gray-300 bg-white py-3 rounded-lg font-medium text-gray-700 hover:bg-gray-50 transition"
>
  <img
    src="https://www.svgrepo.com/show/475656/google-color.svg"
    alt="Google"
    className="w-5 h-5"
  />
  Continue with Google
</button>

        <p className="text-center text-gray-500 text-sm mt-6">
          Don't have an account?
          <a
            href="/register-patient"
            className="ml-1 text-blue-600 hover:text-blue-700 font-medium"
          >
            Register
          </a>
        </p>

        <p className="text-center text-gray-500 text-sm mt-6">
          Forgot Password?
          <a
            href="/forgot-password"
            className="ml-1 text-blue-600 hover:text-blue-700 font-medium"
          >
            Reset Password
          </a>
        </p>
      </div>
    </div>
  );
}

export default LoginForm;