export interface LaboratoryAppointment{
    id: number;
    dateTime: Date;
    laboratoryWorkerId: number;
    supervisorId: number;
    state: LaboratoryAppointmentState; 
    cancelComment: string;
}

export enum LaboratoryAppointmentState {
    Empty = 0,
    Reserved = 1,
    ToBeCompleted = 2,
    WaitingForSupervisor = 3,
    ToBeFixed = 4,
    AllAccepted = 5, //ready for patient
    Finished = 6, //sent to patient
    Cancelled = 7
}