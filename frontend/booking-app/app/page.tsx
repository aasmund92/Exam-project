"use client"
import { FormEvent, useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import CustomAlert from "@/app/alert";

interface Clinics {
  id: number;
  name: string;
  address: string;
  phoneNumber: string;
  email: string;
};

export default function Home() {

    const API_URL = process.env.NEXT_PUBLIC_API_URL;
    const router = useRouter();
    const [search, setSearch] = useState("");
    const [clinicArray, setClinicArray] = useState<Clinics []>([]);
    const [alertType, setAlertType] = useState<"success" | "error">("success");
    const [alertMessage, setAlertMessage] = useState<string | null>(null);
    

  if(!API_URL) {
      throw new Error("API_URL environment variable undefined");
  };

  useEffect(() => {
    const fetchData = async () => {
      try{
        const clinicData = await fetch(`${API_URL}/Clinic`);
        if(!clinicData.ok){
          throw new Error("Error during API request");
        };

      const clinics : Clinics [] = await clinicData.json();
      setClinicArray(clinics);
      }catch(error) {
        console.error("Error while fetching data", error);
      }
    }
    fetchData();
  }, [API_URL]);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    const searchField = search.trim();  
    try{
        if(searchField.length === 0) {
            setAlertType("error");
            setAlertMessage("Search field cannot be empty");
            return;
        }
        const query = {
            query : searchField
            };
        
        const queryParams = new URLSearchParams(query).toString();
        router.push(`/search?${queryParams}`);
    }catch(error) {
        console.error("Error during search", error);
    }
  };  
  

  
    
    
    return (
      <main className="flex min-h-screen flex-col items-center pt-20">
        <h1 className="mb-20 text-4xl font-semibold text-center text-gray-800">
            Clinic Booking Page
        </h1>
        <CustomAlert message={alertMessage} severity={alertType} onClose={() => setAlertMessage(null)}/>
            <div className="w-full max-w-lg flex items-center gap-2">
                <form onSubmit={handleSubmit} className="flex w-full items-center gap-2">
                    <div className="relative flex-grow">
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" className="absolute w-5 h-5 top-2.5 left-2.5 text-slate-600">
                            <path strokeLinecap="round" strokeLinejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
                        </svg>
                        <input 
                            type="text"
                            name="doctorSearch"
                            id="doctorSearch" 
                            className="w-full pl-10 pr-3 py-2 bg-transparent placeholder:text-gray-600 text-gray-600 text-sm border border-slate-200 rounded-md transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-300 shadow-sm focus:shadow" 
                            placeholder="Doctor Search (first name or last name)" 
                            value={search}
                            required
                            onChange={(e) => setSearch(e.target.value)}
                        />
                    </div>
                    <button 
                        className="rounded-md bg-gray-600 py-2 px-4 border border-transparent text-sm text-white transition-all shadow-md hover:shadow-lg focus:bg-slate-700 focus:shadow-none active:bg-slate-700 hover:bg-slate-700 disabled:pointer-events-none disabled:opacity-50"
                        type="submit"
                    >
                        Search
                    </button>
                </form>
            </div>

            
            <div className="grid text-center lg:max-w-5xl lg:w-full gap-4 lg:grid-cols-3 mt-10">
                {clinicArray.map((clinic) => (
                    <Link
                        key={clinic.id}
                        href={`/book/${clinic.id}`}
                        className="bg-white shadow-md rounded-lg group border-transparent px-5 py-4 transition-colors hover:border-gray-300 hover:bg-gray-100 hover:dark:border-neutral-700 hover:dark:bg-neutral-800/30"
                    >
                        <h2 className="mb-3 text-2xl font-semibold text-gray-800">
                            {clinic.name}{" "}
                            <span className="inline-block text-blue-800 transition-transform group-hover:translate-x-1 motion-reduce:transform-none">
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    fill="currentColor"
                                    viewBox="0 0 24 24"
                                    stroke="currentColor"
                                    className="w-5 h-5 inline-block text-gray-800 group-hover:text-gray-600"
                                >
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 5l7 7-7 7" />
                                </svg>
                            </span>
                        </h2>
                        <p className="m-0 max-w-[30ch] text-sm opacity-70 text-gray-800">
                            Address : {clinic.address} <br />
                            Phone Number : {clinic.phoneNumber} <br />
                            Email : {clinic.email}
                        </p>
                    </Link>
                ))}
            </div>
    </main>
    );
};
