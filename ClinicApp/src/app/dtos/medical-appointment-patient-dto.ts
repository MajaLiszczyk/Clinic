export interface MedicalAppointmentPatientDto{
    id: number;
    dateTime: Date;
    patientId: number;
    patientName: string;
    patientSurname: string;
    doctorId: number;
    interview: string;
    diagnosis: string;
    isFinished: boolean;
    isCancelled: boolean;
    cancellingComment: string;
}