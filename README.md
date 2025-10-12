# police_network

The main goal of this project is to help people 'report crimes quickly' without waiting to reach the police station.

#Project overview

In many cities, police stations are far from some areas.  
So when a crime happens, it takes time for police to reach the crime scene.  
This project solves that problem by allowing citizens to send a direct message to nearby police officers using this web app.  

#Purpose
- Reduce the time for police to reach a crime scene.
- Allow people to report crimes directly from anywhere.
- No need to call helpline numbers like 101 — just send a message.
- Help police respond faster by using location information.

#Features
- Users can log in and send messages about crimes.
- Each message contains sender details, receiver details, message content, and location.
- Location can be detected automatically for accuracy.
- Police can view messages in real-time and take necessary actions.
- Users can also file FIRs(First Information Report) and track updates.
- Admin can manage police and thieves(Crimanals) information.
- Dashboard provides analysis of FIRs by type, status, and gender.

#Modules
- Messages (Msg) Module: Core feature for communication between users and police.
- FIR Module: Users can file FIRs with details and assign thieves.
- Police Module: Admin manages police records.
- Thieves Module: Admin tracks criminals and their status.
- Analysis Module: Provides statistics on FIRs for better decision-making.

#Setup Instructions
- For Admin we keep 'Admin@123gmail.com' (In "/Controllers/HomeController.cs"); First Register with this ID for Admin and after login Admin can add Police details.
- After Police login;Poluce can add criminal's details and modify FIR status(Pending to Sucess).
- User can Report FIR and see status of that FIR.
- Refer "Features" portion of README.md(in this file above) for more information.

#Team Members and Individual contributions
- Vahora Atik_CE154 :
    - Messages Module, Police Module, Analysis Module.

- Zala Prashant_CE160 :
    - FIR Module, Thieves Module,Inbox


Note: 
- Here you refer thief as a any crminal, not only crime of thief.
- Please Allow location for Send Message on your machine.
