### Get started

* Download or clone the repository
```
https://github.com/noroff-backend-2/mar24ft-ep2-peterporridge.git
```

* Create your database with mySQL and then modify your appsettings.json file in the Backend directory with your own database name, username and password
```
 "DefaultConnection": "server=localhost;database=yourdbname;user=yourusername;password=yourpassword"
```
```
"DatabaseName": "yourdbname"
```
* Make sure all the packages and dependencies are the correct versions. If some are missing, please add them with this code in the terminal under the Backend directory
```
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.2
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.2
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.2
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
```
* Create database migration with this code in the terminal under the Backend directory
```
dotnet ef migrations add -c ClinicDbContext Initial
```
After the migration is successful, update your database with this code in the terminal under the Backend directory
```
dotnet ef database update
```
* Run the API with this code in the terminal under the Backend directory
```
dotnet run
```
* Create an .env file in the Frontend/clinic-booking-app directory. Set your .env file with the string below. Change with your own API URL, for example localhost:3000/api
```
NEXT_PUBLIC_API_URL = your API URL
```
* Make sure all the dependencies in Frontend/clinic-booking-app directory is installed. If some are missing, please add them with this code in the terminal in the Frontend/clinic-booking-app directory
```
npm install @mui/material
npm install dotenv
npm install lucide-react
npm install next
npm install react
npm install react-dom
npm install @emotion/react
npm install @emotion/styled 
```
* Install the dependencies with this code in the terminal under the Frontend/clinic-booking-app directory
```
npm install
```
* Run the app with this code in the terminal under the Frontend/clinic-booking-app directory
```
npm run dev
```
* Swagger docs can be reached here
```
https://localhost:YOURROUTE/doc
```

### ENDPOINTS
#### localhost:3000/
```
This is the root endpoint of the frontend app. Here the patient can search for doctors, or choose which clinic he wants to book an appointment with.
```
#### localhost:3000/book/{id}
```
Patient booking form based on which clinic was chosen
```
#### localhost:3000/confirmation
```
Confirmation page with details about the appointment the patient booked
```
#### localhost:3000/search
```
Search page with details about the doctor(s) the patient searched for
```
#### /api/Appointment GET
```
Get all appointments
```
#### /api/Appointment POST
```
Creates a new appointment
```
#### /api/Appointment/{Id} GET
```
Get one specific appointment
```
#### /api/Appointment/{Id} PUT
```
Updates a specific appointment
```
#### /api/Appointment/{Id} DELETE
```
Deletes a specific appointment
```
#### /api/Category GET
```
Get all categories
```
#### /api/Category POST
```
Creates a new category
```
#### /api/Category/{Id} GET
```
Get one specific category
```
#### /api/Category/{Id} PUT
```
Updates a specific category
```
#### /api/Category/{Id} DELETE
```
Deletes a specific category
```
#### /api/Clinic GET
```
Get all clinics
```
#### /api/Clinic POST
```
Creates a new clinic
```
#### /api/Clinic/{Id} GET
```
Get one specific clinic
```
#### /api/Clinic/{Id} PUT
```
Updates a specific clinic
```
#### /api/Clinic/{Id} DELETE
```
Deletes a specific clinic
```
#### /api/Doctor GET
```
Get all doctors
```
#### /api/Doctor POST
```
Creates a new doctor
```
#### /api/Doctor/{Id} GET
```
Get one specific doctor
```
#### /api/Doctor/{Id} PUT
```
Updates a specific doctor
```
#### /api/Doctor/{Id} DELETE
```
Deletes a specific doctor
```
#### /api/Doctor/Search GET
```
Search for a specific doctor through query parameter
```
#### /api/Gender GET
```
Get all genders
```
#### /api/Gender POST
```
Creates a new gender
```
#### /api/Gender/{Id} GET
```
Get one specific gender
```
#### /api/Gender/{Id} PUT
```
Updates a specific gender
```
#### /api/Gender/{Id} DELETE
```
Deletes a specific gender
```
#### /api/Patient GET
```
Get all patients
```
#### /api/Patient POST
```
Creates a new patient
```
#### /api/Patient/{Id} GET
```
Get one specific patient
```
#### /api/Patient/{Id} PUT
```
Updates a specific patient
```
#### /api/Patient/{Id} DELETE
```
Deletes a specific patient
```
#### /api/Religion GET
```
Get all religions
```
#### /api/Religion POST
```
Creates a new religion
```
#### /api/Religion/{Id} GET
```
Get one specific religion
```
#### /api/Religion/{Id} PUT
```
Updates a specific religion
```
#### /api/Religion/{Id} DELETE
```
Deletes a specific religion
```
#### /api/Speciality GET
```
Get all specialties
```
#### /api/Speciality POST
```
Creates a new speciality
```
#### /api/Speciality/{Id} GET
```
Get one specific speciality
```
#### /api/Speciality/{Id} PUT
```
Updates a specific speciality
```
#### /api/Speciality/{Id} DELETE
```
Deletes a specific speciality
```


### REFERENCES

Tailwind form - https://flowbite.com/docs/components/forms/ <br>
Tailwind navbar - https://flowbite.com/docs/components/navbar/ https://www.material-tailwind.com/docs/html/navbar<br>
Tailwind input - https://flowbite.com/docs/forms/input-field/ <br>
Tailwind timepicker - https://flowbite.com/docs/forms/timepicker/ <br>
Tailwind svg icons - https://flowbite.com/docs/customize/icons/ https://flowbite.com/icons/<br>
Data transfer object(DTOs) - https://learn.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5<br>
