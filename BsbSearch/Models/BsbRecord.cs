﻿namespace BsbSearch.Models
{
    public record BsbRecord(
        string Id, 
        string Number, 
        string FICode, 
        string Name, 
        string Address, 
        string Suburb, 
        string State, 
        string PostCode, 
        string PaymentSystems
        );
}
