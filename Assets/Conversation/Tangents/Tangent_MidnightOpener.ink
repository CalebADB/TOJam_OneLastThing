
#Pers: Andy
I'm heading to bed.
->Start

=== Start ===
 * [Is she going to go to bed? # Pers: Alex]
    # Pers: Alex
    You're going to bed? #SichInc: V_AndyAnger_1.3
    # Pers: Andy
    Yeah why do you ask? #SichInc: V_AlexAnger_1.3
 * [Are they okay? # Pers: Andy #SichMinReq: V_AndyExhausted_2.9]
    # Pers: Andy
    Are you doing okay? #SichInc: V_AndyAnger_1.3
    # Pers: Alex
    I mean I got all these dishes.
    Taking my time #SichInc: V_AndyFear_2.3
    # Pers: Andy
    Just come to bed! #SichInc: V_AlexAnger_1.3
 + [That bag wasn't the solution I needed. # Pers: Alex]
    -> ALoan
 - I'm not ready for bed. # Pers: Alex
#Pers: Andy
But I need to go to bed. #SichInc: V_AlexAnger_0.5
-> Start
->DONE
 
=== ALoan === 
NULL # Pers: Alex #StartTangent: ALoan
+ [I want to go to bed #Pers: Andy #SichMinReq: V_AndyExhausted_4.9]
    #Pers: Andy
    Ok, I need to go to bed.

-> Start
