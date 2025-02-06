export interface MedicalAppointment{
    id: number;
    dateTime: Date;
    patientId: number;
    doctorId: number;
    interview: string;
    diagnosis: string;
    isFinished: boolean;
    isCancelled: boolean;
    cancellingComment: string;
}