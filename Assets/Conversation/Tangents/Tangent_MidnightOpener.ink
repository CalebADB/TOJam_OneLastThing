
#Pers: Andy
I'm heading to bed.#SichInc: V_AlexExhausted_1.3
->Start

=== Start ===
 * [Is she going to go to bed? # Pers: Alex]
    # Pers: Alex
    You're going to bed? #SichInc: V_AndyFear_1.3
    # Pers: Andy
    Yeah why do you ask? #SichInc: V_AlexFear_1.3
 * [Are they okay? # Pers: Andy #SichMinReq: V_AndyExhausted_2.9]
    # Pers: Andy
    Are you doing okay? #SichInc: V_AndyFear_1.3
    # Pers: Alex
    I mean I got all these dishes.
    Taking my time #SichInc: V_AndyFear_2.3
    # Pers: Andy
    Just come to bed! #SichInc: V_AlexFear_1.3
 + [That bag wasn't the solution I needed. # Pers: Alex]
    -> ALoan
 - I'm not ready for bed. # Pers: Alex
#Pers: Andy
But I need to go to bed. #SichInc: V_AlexFear_0.5
-> Start
->DONE
 
=== ALoan === 
NULL # Pers: Alex #StartTangent: ALoan
+ [I want to go to bed #Pers: Andy #SichMinReq: V_AndyExhausted_4.9]
    #Pers: Andy
    Ok, I need to go to bed.

-> Start
