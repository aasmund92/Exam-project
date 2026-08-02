"use client";

import { useSearchParams, useRouter } from "next/navigation";
import { CheckCircle } from "lucide-react";

export default function ConfirmationPage () {
    const searchParams = useSearchParams();
    const router = useRouter();

    
    const firstName = searchParams.get("firstName") || "N/A";
    const lastName = searchParams.get("lastName") || "N/A";
    const doctorFirstName = searchParams.get("doctorFirstName") || "N/A";
    const doctorLastName = searchParams.get("doctorLastName") || "N/A";
    const appointmentDate = searchParams.get("appointmentDate") || "N/A";
    const appointmentTime = searchParams.get("appointmentTime") || "N/A";
    const clinicName = searchParams.get("clinicName") || "N/A";

    return (
        <div className="max-w-md mx-auto p-6 bg-white shadow-md rounded-lg mt-5 pt-20">
            <h1 className="text-2xl font-bold mb-4 text-center text-gray-800">Appointment Booked at {clinicName}</h1>
            <p className="text-lg text-gray-600">
                Thank you for booking with us at {clinicName}!<br/><strong>Booking Details:</strong>
            </p>
            <p className="text-lg text-gray-600">
                <strong>Patient Name:</strong> {firstName} {lastName}
            </p>
            <p className="text-lg text-gray-600">
                <strong>Doctor Name:</strong> {doctorFirstName} {doctorLastName}
            </p>
            <p className="text-lg text-gray-600">
                <strong>Date:</strong> {appointmentDate}
            </p>
            <p className="text-lg text-gray-600">
                <strong>Time:</strong> {appointmentTime}
            </p>
            <div className="flex justify-center mt-6">
                <CheckCircle className="text-green-500 w-12 h-12" />
            </div>
            <div className="mt-4 text-center">
                <button
                    onClick={() =>  router.push("/")}
                    className="bg-gray-800 text-white px-4 py-2 rounded"
                >
                    Home
                </button>
            </div>
        </div>
    );
}