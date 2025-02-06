
import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PatientComponent } from './patient/patient.component';
import { RegistrantPatientsComponent } from './registrant-patients/registrant-patients.component';
import { RegistrantComponent } from './registrant/registrant.component';
import { RegistrantSpecialisationsComponent } from './registrant-specialisations/registrant-specialisations.component';
import { RegistrantDiagnosticTestTypesComponent } from './registrant-diagnostic-test-types/registrant-diagnostic-test-types.component';
import { RegistrantDoctorsComponent } from './registrant-doctors/registrant-doctors.component';
import { RegistrantAppointmentsComponent } from './registrant-appointments/registrant-appointments.component';
import { DoctorAppointmentsComponent } from './doctor-appointments/doctor-appointments.component';
import { AppointmentDetailsComponent } from './appointment-details/appointment-details.component';
import { RegistrantLaboratoryWorkersComponent } from './registrant-laboratory-workers/registrant-laboratory-workers.component';
import { RegistrantLaboratorySupervisorsComponent } from './registrant-laboratory-supervisors/registrant-laboratory-supervisors.component';
import { RegistrantLaboratoryTestTypesComponent } from './registrant-laboratory-test-types/registrant-laboratory-test-types.component';
import { RegistrantLaboratoryAppointmentsComponent } from './registrant-laboratory-appointments/registrant-laboratory-appointments.component';
import { PatientMenuComponent } from './patient-menu/patient-menu.component';

import { LoginComponent } from './log-in/log-in.component';
import { AuthorizationGuard } from './authorization/authorization-guard';
import { PatientLaboratoryAppointmentsComponent } from './patient-laboratory-appointments/patient-laboratory-appointments.component';
import { LaboratoryWorkerComponent } from './laboratory-worker/laboratory-worker.component';
import { LaboratoryAppointmentDetailsComponent } from './laboratory-appointment-details/laboratory-appointment-details.component';
import { LaboratoryAppointmentDetailsSupervisorComponent } from './laboratory-appointment-details-supervisor/laboratory-appointment-details-supervisor.component';
import { LaboratorySupervisorComponent } from './laboratory-supervisor/laboratory-supervisor.component';
import { AdminComponent } from './admin/admin.component';


export const routes: Routes = [
    { path: '', component: LoginComponent },
    { path: 'log-in', component: LoginComponent },
    { path: 'patient/:patientId/:registrantId', component: PatientComponent, canActivate: [AuthorizationGuard] },
    { path: 'patient-menu/:patientId', component: PatientMenuComponent, canActivate: [AuthorizationGuard] },
    { path: 'patient-laboratory-appointments/:patientId/:registrantId', component: PatientLaboratoryAppointmentsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant/:registrantId', component: RegistrantComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-patients/:registrantId', component: RegistrantPatientsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-specialisations/:registrantId', component: RegistrantSpecialisationsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-diagnostic-test-types/:registrantId', component: RegistrantDiagnosticTestTypesComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-doctors/:registrantId', component: RegistrantDoctorsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-laboratory-workers/:registrantId', component: RegistrantLaboratoryWorkersComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-laboratory-supervisors/:registrantId', component: RegistrantLaboratorySupervisorsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-appointments/:registrantId', component: RegistrantAppointmentsComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-laboratory-test-types/:registrantId', component: RegistrantLaboratoryTestTypesComponent, canActivate: [AuthorizationGuard] },
    { path: 'registrant-laboratory-appointments/:registrantId', component: RegistrantLaboratoryAppointmentsComponent, canActivate: [AuthorizationGuard] },
    { path: 'laboratory-worker/:laboratoryWorkerId/:registrantId', component: LaboratoryWorkerComponent, canActivate: [AuthorizationGuard] },
    { path: 'laboratory-supervisor/:laboratorySupervisorId/:registrantId', component: LaboratorySupervisorComponent, canActivate: [AuthorizationGuard] },
    { path: 'admin', component: AdminComponent, canActivate: [AuthorizationGuard] },
    { path: 'doctor-appointments/:doctorId/:registrantId', component: DoctorAppointmentsComponent, canActivate: [AuthorizationGuard] },
    { path: 'appointment-details/:id', component: AppointmentDetailsComponent, },
    { path: 'laboratory-appointment-details/:id', component: LaboratoryAppointmentDetailsComponent, },
    { path: 'laboratory-appointment-details-supervisor/:id', component: LaboratoryAppointmentDetailsSupervisorComponent, },

    { path: '**', redirectTo: '' }, // Przekierowanie na stronę główną dla nieznanych tras
    { path: '', redirectTo: '/your-data', pathMatch: 'full' },

];
