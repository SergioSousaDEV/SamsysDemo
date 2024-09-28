export interface ClientCreateDTO {
    name: string;
    phoneNumber: string;
    isActive: boolean;
    dateOfBirth?: Date | null;
}