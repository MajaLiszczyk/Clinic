import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { YourDataComponent } from './your-data/your-data.component';
import { AddPatientComponent } from './add-patient/add-patient.component';
import { AddDoctorComponent } from './add-doctor/add-doctor.component';
import { AddMedicalAppointmentComponent } from './add-medical-appointment/add-medical-appointment.component';
import { AddSpecialisationComponent } from './add-specialisation/add-specialisation.component';
import { MakeAnAppointmentComponent } from './make-an-appointment/make-an-appointment.component';
import { GetPatientsComponent } from './get-patients/get-patients.component';
import { GetDoctorsComponent } from './get-doctors/get-doctors.component';
import { GetSpecialisationsComponent } from './get-specialisations/get-specialisations.component';
import { GetMedicalAppointmentsComponent } from './get-medical-appointments/get-medical-appointments.component';
import { GetRegistrantsComponent } from './get-registrants/get-registrants.component';
import { AddRegistrantComponent } from './add-registrant/add-registrant.component';
import { GetMedicalAppointmentsForPatientComponent } from './get-medical-appointments-for-patient/get-medical-appointments-for-patient.component';
import { PatientComponent } from './patient/patient.component';
import { PatientParentComponent } from './patient-parent/patient-parent.component';
//import { RegistrantPatientsOldComponent } from './registrant-patients-old/registrant-patients-old.component';
import { RegistrantPatientsComponent } from './registrant-patients/registrant-patients.component';

import { RegistrantComponent } from './registrant/registrant.component';
//import { RegistrantSpecialisationsOldComponent } from './registrant-specialisations-old/registrant-specialisations-old.component';
import { RegistrantSpecialisationsComponent } from './registrant-specialisations/registrant-specialisations.component';
//import { RegistrantDoctorsComponent } from './registrant-doctors-old/registrant-doctors-old.component';
import { RegistrantDoctorsComponent } from './registrant-doctors/registrant-doctors.component';

import { RegistrantAppointmentsComponent } from './registrant-appointments/registrant-appointments.component';
import { DoctorAppointmentsComponent } from './doctor-appointments/doctor-appointments.component';
import { DoctorComponent } from './doctor/doctor.component';
import { AppointmentDetailsComponent } from './appointment-details/appointment-details.component';


export const routes: Routes = [
    //{ path: '', component: AppComponent }, // Strona główna
    { path: 'your-data', component: YourDataComponent }, // Strona "your-data"
    { path: 'add-patient', component: AddPatientComponent }, // Strona "Add Patient"
    { path: 'add-doctor', component: AddDoctorComponent }, // Strona "Add Patient"
    { path: 'add-medical-appointment', component: AddMedicalAppointmentComponent }, // Strona "Add Patient"
    { path: 'add-specialisation', component: AddSpecialisationComponent},
    { path: 'your-data', component: AddMedicalAppointmentComponent }, // Strona "Add Patient"
    { path: 'make-an-appointment', component: MakeAnAppointmentComponent},
    { path: 'get-patients', component: GetPatientsComponent},
    { path: 'get-doctors', component: GetDoctorsComponent},
    { path: 'get-specialisations', component: GetSpecialisationsComponent},
    { path: 'get-medical-appointments', component: GetMedicalAppointmentsComponent},
    { path: 'get-registrants', component: GetRegistrantsComponent},
    { path: 'add-registrant', component: AddRegistrantComponent},
    { path: 'get-medical-appointments-for-patient', component: GetMedicalAppointmentsForPatientComponent},
    { path: 'patient/:patientId', component: PatientComponent},
    { path: 'patient-parent', component: PatientParentComponent},
    { path: 'registrant', component: RegistrantComponent},
    { path: 'registrant-patients', component: RegistrantPatientsComponent},
    //{ path: 'registrant-specialisations-old', component: RegistrantSpecialisationsOldComponent},
    { path: 'registrant-specialisations', component: RegistrantSpecialisationsComponent},
    { path: 'registrant-doctors', component: RegistrantDoctorsComponent},
    { path: 'registrant-appointments', component: RegistrantAppointmentsComponent},
    { path: 'doctor', component: DoctorComponent},
    { path: 'doctor-appointments/:doctorId', component: DoctorAppointmentsComponent},
    { path: 'appointment-details/:id', component: AppointmentDetailsComponent},

    { path: '**', redirectTo: '' }, // Przekierowanie na stronę główną dla nieznanych tras
    { path: '', redirectTo: '/your-data', pathMatch: 'full' }, // Przekierowanie do konkretnej trasy

];
