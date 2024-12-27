export interface DoctorWithSpcecialisations{
    id: number;
    name: string;
    surname: string;
    doctorNumber: string;
    specialisationIds: number[];
    isAvailable: boolean;
}