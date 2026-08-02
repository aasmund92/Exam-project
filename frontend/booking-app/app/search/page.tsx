"use client"
import { useEffect, useState } from "react";
import { useSearchParams, useRouter } from "next/navigation";

interface Doctors {
    id: number;
    firstName: string;
    lastName: string;
    specialityName : string;
    clinicName: string;
}

export default function Search () {
    const API_URL = process.env.NEXT_PUBLIC_API_URL;
    const searchParams = useSearchParams();
    const router = useRouter();
    
    if(!API_URL) {
        throw new Error("API_URL environment variable undefined");
    };
    
    const [doctorsArray, setDoctorsArray] = useState<Doctors []>([]);

    const searchQuery = searchParams.get("query");

    useEffect(() => {
        const fetchData = async () => {
            try {
                const doctorSearchData = await fetch(`${API_URL}/Doctor/Search/?query=${searchQuery}`);

                if(doctorSearchData.status === 404) {
                    return setDoctorsArray([]);
                  }
                
                if(!doctorSearchData.ok) {
                    throw new Error("API request failed");
                }
                const doctors : Doctors [] = await doctorSearchData.json();

                setDoctorsArray(doctors);
            }catch(error) {
                console.error("Error while fetching data", error);
            };
        };
        fetchData();
       
    }, [API_URL, searchQuery]);

    return (

        <div className="max-w-5xl mx-auto p-6 mt-10 pt-20">
            <h1 className="text-2xl font-bold mb-4 text-center text-gray-800">Search Results</h1>

            {doctorsArray.length > 0 ? (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {doctorsArray.map((doctor) => (
                        <div key={doctor.id} className="bg-white p-4 rounded-lg shadow-md">
                            <p className="text-lg text-gray-600">
                                <strong>Doctor Name:</strong> {doctor.firstName} {doctor.lastName}
                            </p>
                            <p className="text-lg text-gray-600">
                                <strong>Clinic:</strong> {doctor.clinicName} 
                            </p>
                            <p className="text-lg text-gray-600">
                                <strong>Speciality:</strong> {doctor.specialityName}
                            </p>
                        </div>
                    ))}
                </div>
            ) : (
                <p className="text-lg text-gray-600 text-center">
                    No doctors found
                </p>
            )}

            <div className="mt-4 text-center">
                <button
                    onClick={() => router.push("/")}
                    className="bg-gray-800 text-white px-4 py-2 rounded"
                >
                    Home
                </button>
            </div>
        </div>
    )
};