"use client";

import { useParams, useRouter } from "next/navigation";
import { useState, useEffect, FormEvent } from "react";
import CustomAlert from "@/app/alert";
import formatInput from "@/app/formatInput";


interface Doctors {
    id: number;
    firstName: string;
    lastName: string;
    specialityId: number;
    clinicId: number;
};

interface Patients {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    genderId : number;
    religionId : number;
};

interface Clinic {
    id: number;
    name: string;
    address: string;
    phoneNumber: string;
    email: string;
};

interface Genders {
    id: number;
    name: string;
};

interface Religions {
    id: number;
    name: string;
};

interface Categories {
    id: number;
    name: string;
};

export default function Book() {
    const router = useRouter();
    const params = useParams();
    const clinicId = Number(params.id);

    const [appointmentTime, setAppointmentTime] = useState("");
    const [appointmentDate, setAppointmentDate] = useState("");
    const [duration, setDuration] = useState("");
    const [clinicName, setClinicName] = useState("");
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [birthday, setBirthday] = useState("");
    const [categoryId, setCategoryId] = useState<number | null>(null);
    const [doctorId, setDoctorId] = useState<number | null>(null);
    const [genderId, setGenderId] = useState<number | null>(null);
    const [religionId, setReligionId] = useState<number | null>(null);
    const [genderArray, setGenderArray] = useState<Genders []>([]);
    const [religionArray, setReligionArray] = useState<Religions []>([]);
    const [doctorArray, setDoctorArray] = useState<Doctors []>([]);
    const [categoryArray, setCategoryArray] = useState<Categories []>([]);
    const [alertType, setAlertType] = useState<"success" | "error">("success");
    const [alertMessage, setAlertMessage] = useState<string | null>(null);
    
    
    const API_URL = process.env.NEXT_PUBLIC_API_URL;

    if(!API_URL) {
        throw new Error("API_URL environment variable undefined");
    };

    useEffect(() => {
        const fetchData = async () => {
            
            try{
                const [clinicData, doctorData, genderData, religionData, categoryData] = await Promise.all([
                    fetch(`${API_URL}/Clinic/${clinicId}`),
                    fetch(`${API_URL}/Doctor`),
                    fetch(`${API_URL}/Gender`),
                    fetch(`${API_URL}/Religion`),
                    fetch(`${API_URL}/Category`)
                ]);
                
                if(!doctorData.ok || !clinicData.ok || !genderData.ok || !religionData.ok || !categoryData.ok){
                    throw new Error("one or more API requests failed");  
                };
                const doctors : Doctors[] = await doctorData.json();
                const clinic : Clinic = await clinicData.json();
                const gender : Genders [] = await genderData.json();
                const religion : Religions [] = await religionData.json();
                const category : Categories [] = await categoryData.json();
                
                const clinicDoctors = doctors.filter(doctor => doctor.clinicId === clinicId);

                setDoctorArray(clinicDoctors);
                setGenderArray(gender);
                setReligionArray(religion);
                setCategoryArray(category);
                setClinicName(clinic.name);
                console.log("fetch reached");
            }catch(error){
                console.error("Error while fetching data", error);
            };
        };
        fetchData();
    }, [API_URL , clinicId]);

    const timeSpanDuration = (minutes: number) => {
        const hours = Math.floor(minutes/60);
        const mins = minutes % 60;
        return `${String(hours).padStart(2, "0")}:${String(mins).padStart(2, "0")}:00`;
    };

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();
        
        try{
            const capitalizedFirstName = await formatInput(firstName, "name");
            const capitalizedLastName = await formatInput(lastName, "name");
            const lowerCaseEmail = await formatInput(email, "email");
            const formattedDuration = timeSpanDuration(Number(duration));
            const patientData = await fetch(`${API_URL}/Patient`);
            
            if(!patientData.ok) {
                throw new Error("API request failed");
            };
            if(lowerCaseEmail == "invalid") {
                throw new Error("Invalid email format");
            };

            if(capitalizedFirstName == "invalid") {
                throw new Error("Fist name cannot be empty");
            };

            if(capitalizedLastName == "invalid") {
                throw new Error("Last name  cannot be empty");
            };
            
            
            const patients : Patients [] = await patientData.json();
            

            

            const existingPatient = patients.find(patient => patient.email === lowerCaseEmail && patient.firstName === capitalizedFirstName && patient.lastName === capitalizedLastName);
            if(existingPatient) {
                const patientId = existingPatient.id;
                const appointmentResponse = await fetch(`${API_URL}/Appointment`, {
                    method: "POST",
                    headers: {
                        "Content-Type" : "application/json",
                    },
                    body: JSON.stringify({
                        appointmentTime: `${appointmentDate}T${appointmentTime}`,
                        patientId : patientId,
                        categoryId : categoryId,
                        doctorId : doctorId,
                        duration : formattedDuration
                    }),
                });
                if(!appointmentResponse.ok) {
                    const errorText = await appointmentResponse.text();
                    throw new Error(errorText || "Failed to create appointment for already existing patient");
                };
                const doctor = doctorArray.find(d => d.id === doctorId)
                const bookingDetails = {
                    firstName : capitalizedFirstName,
                    lastName : capitalizedLastName,
                    doctorFirstName : doctor?.firstName ?? "",
                    doctorLastName : doctor?.lastName ?? "",
                    clinicName : clinicName,
                    appointmentDate : appointmentDate,
                    appointmentTime : appointmentTime 
                };
                
                const queryParams = new URLSearchParams(bookingDetails).toString();
                router.push(`/confirmation?${queryParams}`);
                
                
            }else {
                const patientResponse = await fetch(`${API_URL}/Patient`, {
                    method : "POST",
                    headers: {
                        "Content-Type" : "application/json",
                    },
                    body: JSON.stringify({
                        firstName : capitalizedFirstName,
                        lastName : capitalizedLastName,
                        email: lowerCaseEmail,
                        birthday : birthday,
                        genderId : genderId,
                        religionId : religionId
                    }),
                });

                if(!patientResponse.ok) {
                    const errorText = await patientResponse.text();
                    throw new Error(errorText || "Failed to create patient");
                };

                const newPatient : Patients = await patientResponse.json();
                const newPatientId = newPatient.id;

                const appointmentResponse = await fetch(`${API_URL}/Appointment`, {
                    method : "POST",
                    headers : {
                        "Content-Type" : "application/json",
                    },
                    body: JSON.stringify({
                        appointmentTime : `${appointmentDate}T${appointmentTime}`,
                        patientId : newPatientId,
                        categoryId : categoryId,
                        doctorId : doctorId,
                        duration: formattedDuration
                    }),
                });
                if(!appointmentResponse.ok) {
                    const errorText = await appointmentResponse.text();
                    throw new Error(errorText || "Failed to create appointment");
                };
                const doctor = doctorArray.find(d => d.id === doctorId)
                const bookingDetails = {
                    firstName : capitalizedFirstName,
                    lastName : capitalizedLastName,
                    doctorFirstName : doctor?.firstName ?? "",
                    doctorLastName : doctor?.lastName ?? "",
                    clinicName : clinicName,
                    appointmentDate : appointmentDate,
                    appointmentTime : appointmentTime 
                };
                
                const queryParams = new URLSearchParams(bookingDetails).toString();
                router.push(`/confirmation?${queryParams}`);
            };
            

        }catch(error: unknown) {
            setAlertType("error");
            
            if (error instanceof Error) {
                setAlertMessage(error.message); 
            } else {
                setAlertMessage("An unknown error occurred"); 
            };

            console.error("Error during API request", error);
        };
    };

    return (
        <div className="flex min-h-screen flex-col items-center pt-20">
            <h1 className="mb-20 text-4xl font-semibold text-center text-gray-800">
                Book appointment at {clinicName}
            </h1>
            <CustomAlert message={alertMessage} severity={alertType} onClose={() => setAlertMessage(null)}/>

            <form className="max-w-md mx-auto grid grid-cols-1 gap-4 md:grid-cols-2" onSubmit={handleSubmit}>
                <div className="relative z-0 w-full mb-5 group">
                <input
                    type="text"
                    name="firstName"
                    id="firstName"
                    className="block py-2.5 px-0 w-full text-sm text-gray-900 bg-transparent border-0 border-b-2 border-gray-300 appearance-none  dark:border-gray-600 dark:focus:border-gray-500 focus:outline-none focus:ring-0 focus:border-gray-600 peer"
                    placeholder=" "
                    required
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                />
                <label className="peer-focus:font-medium absolute text-sm text-gray-500 dark:text-gray-400 duration-300 transform -translate-y-6 scale-75 top-3 -z-10 origin-[0] peer-focus:start-0 rtl:peer-focus:translate-x-1/4 rtl:peer-focus:left-auto peer-focus:text-gray-600 peer-focus:dark:text-gray-500 peer-placeholder-shown:scale-100 peer-placeholder-shown:translate-y-0 peer-focus:scale-75 peer-focus:-translate-y-6">
                    First Name
                </label>
                </div>

                <div className="relative z-0 w-full mb-5 group">
                <input
                    type="text"
                    name="lastName"
                    id="lastName"
                    className="block py-2.5 px-0 w-full text-sm text-gray-900 bg-transparent border-0 border-b-2 border-gray-300 appearance-none dark:border-gray-600 dark:focus:border-gray-500 focus:outline-none focus:ring-0 focus:border-gray-600 peer"
                    placeholder=" "
                    required
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                />
                <label className="peer-focus:font-medium absolute text-sm text-gray-500 dark:text-gray-400 duration-300 transform -translate-y-6 scale-75 top-3 -z-10 origin-[0] peer-focus:start-0 rtl:peer-focus:translate-x-1/4 rtl:peer-focus:left-auto peer-focus:text-gray-600 peer-focus:dark:text-gray-500 peer-placeholder-shown:scale-100 peer-placeholder-shown:translate-y-0 peer-focus:scale-75 peer-focus:-translate-y-6">
                    Last Name
                </label>
                </div>

                <div className="relative z-0 w-full mb-5 group">
                <input
                    type="email"
                    name="email"
                    id="email"
                    className="block py-2.5 px-0 w-full text-sm text-gray-900 bg-transparent border-0 border-b-2 border-gray-300 appearance-none dark:border-gray-600 dark:focus:border-gray-500 focus:outline-none focus:ring-0 focus:border-gray-600 peer"
                    placeholder=" "
                    required
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <label className="peer-focus:font-medium absolute text-sm text-gray-500 dark:text-gray-400 duration-300 transform -translate-y-6 scale-75 top-3 -z-10 origin-[0] peer-focus:start-0 rtl:peer-focus:translate-x-1/4 rtl:peer-focus:left-auto peer-focus:text-gray-600 peer-focus:dark:text-gray-500 peer-placeholder-shown:scale-100 peer-placeholder-shown:translate-y-0 peer-focus:scale-75 peer-focus:-translate-y-6">
                    Email
                </label>
                </div>

                <div className="relative z-0 w-full mb-5 group">
                    <label className="text-sm text-gray-500 dark:text-gray-400">
                        Date of Birth
                    </label>
                    <input
                        id="birthday"
                        type="date"
                        className="peer bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500" 
                        placeholder="Select birthdate"
                        value={birthday}
                        required
                        onChange={(e) => setBirthday(e.target.value)}
                    />
                </div>

                <div className="mb-4">
                <select
                    name="gender_id"
                    id="gender_id"
                    value={genderId ?? ""}
                    required
                    onChange={(e) => setGenderId(Number(e.target.value))}
                    className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500"
                >
                    <option value="" disabled>Select a gender</option>
                    {genderArray.map((gender) => (
                    <option key={gender.id} value={gender.id}>
                        {gender.name}
                    </option>
                    ))}
                </select>
                </div>

                <div className="mb-4">
                <select
                    name="religion_id"
                    id="religion_id"
                    value={religionId ?? ""}
                    required
                    onChange={(e) => setReligionId(Number(e.target.value))}
                    className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500"
                >
                    <option value="" disabled>Select a religion</option>
                    {religionArray.map((religion) => (
                    <option key={religion.id} value={religion.id}>
                        {religion.name}
                    </option>
                    ))}
                </select>
                </div>

                <div className="mb-4">
                <select
                    name="doctor_id"
                    id="doctor_id"
                    value={doctorId ?? ""}
                    required
                    onChange={(e) => setDoctorId(Number(e.target.value))}
                    className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500"
                >
                    <option value="" disabled>Select a doctor</option>
                    {doctorArray.map((doctor) => (
                    <option key={doctor.id} value={doctor.id}>
                        {doctor.firstName} {doctor.lastName}
                    </option>
                    ))}
                </select>
                </div>

                <div className="mb-4">
                <select
                    name="category_id"
                    id="category_id"
                    value={categoryId ?? ""}
                    required
                    onChange={(e) => setCategoryId(Number(e.target.value))}
                    className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500"
                >
                    <option value="" disabled>Select a category</option>
                    {categoryArray.map((category) => (
                    <option key={category.id} value={category.id}>
                        {category.name}
                    </option>
                    ))}
                </select>
                </div>

                <div className="relative z-0 w-full mb-5 group">
                    <label className="text-sm text-gray-500 dark:text-gray-400">
                        Appointment Date
                    </label>
                    <input 
                        id="appointmentTimeDate" 
                        type="date" 
                        className="peer bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500" 
                        placeholder="Select date"
                        required
                        min={new Date().toISOString().split("T")[0]}
                        value={appointmentDate}
                        onChange={(e) => setAppointmentDate(e.target.value)} 
                    />
                </div>

                <div className="relative z-0 w-full mb-5 group">
                    <label className="text-sm text-gray-500 dark:text-gray-400">
                        Appointment Time
                    </label>
                    <input 
                        type="time" 
                        id="appointmentTime" 
                        className="peer bg-gray-50 border leading-none border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500" 
                        min="08:00" 
                        max="16:00" 
                        value={appointmentTime} 
                        required
                        onChange={(e) => setAppointmentTime(e.target.value)}
                    />
                </div>
                
                <div className="relative z-0 w-full mb-5 group">
                    <label className="text-sm text-gray-500 dark:text-gray-400">
                        Appointment Duration
                    </label>
                    <input 
                        type="number" 
                        id="number-input" 
                        className="peer bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500" 
                        placeholder="Duration (min)" 
                        required
                        value={duration}
                        onChange={(e) => setDuration(e.target.value)}
                    />
                </div>
                <div className="relative z-0 w-full mb-5 group col-span-full">
                    <button
                        type="submit"
                        className="bg-gray-800 text-white px-2 py-2 rounded-lg hover:bg-gray-700 transition duration-300"
                    >
                        Book
                </button>
                <button
                        type="button"
                        onClick={() => router.push("/")}
                        className="bg-gray-800 text-white px-2 py-2 rounded-lg hover:bg-gray-700 transition duration-300 mx-2"
                    >
                        Back
                </button>
              </div>
        </form>
    </div>
    )
};
