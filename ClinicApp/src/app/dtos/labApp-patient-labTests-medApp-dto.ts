import { LaboratoryAppointmentState } from "../model/laboratory-appointment";
import { LaboratoryTest } from "../model/laboratory-test";

export interface LabAppWithPatientLabTestsMedApp{
    medicalAppointmentId: number;
    medicalAppointmentDateTime: string;
    doctorId: number;
    doctorName: string;
    doctorSurname: string;
    laboratoryAppointmentId: number;
    laboratoryWorkerId: number;
    supervisorId: number;
    state: LaboratoryAppointmentState; // wczesniej byl string
    dateTime: string;
    cancelComment: string | null;
    patientId: number;
    patientName: string;
    patientSurname: string;
    patientPesel: string;
    laboratoryTests: LaboratoryTest[];
}
