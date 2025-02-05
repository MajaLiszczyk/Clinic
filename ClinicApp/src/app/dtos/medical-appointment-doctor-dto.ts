export interface MedicalAppointmentDoctorDto{
    id: number;
    dateTime: Date;
    patientId: number;
    doctorId: number;
    doctorName: string;
    doctorSurname: string;
    interview: string;
    diagnosis: string;
    diseaseUnit: number;
    isFinished: boolean;
    isCancelled: boolean;
    cancellingComment: string;
}