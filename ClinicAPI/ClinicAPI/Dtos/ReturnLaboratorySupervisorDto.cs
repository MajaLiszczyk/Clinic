﻿namespace ClinicAPI.Dtos
{
    public class ReturnLaboratorySupervisorDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LaboratorySupervisorNumber { get; set; }
        public bool isAvailable { get; set; }
    }
}
