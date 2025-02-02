import { LaboratoryAppointmentState } from "../model/laboratory-appointment";

export interface LaboratoryAppointmentWorkerSupervisor{
    id: number;
    dateTime: string;
    laboratoryWorkerId: number;
    laboratoryWorkerName: string;
    laboratoryWorkerSurname: string;
    supervisorId: number;
    supervisorName: string;
    supervisorSurname: string;
    cancelComment: string | null;
    state: LaboratoryAppointmentState;
}