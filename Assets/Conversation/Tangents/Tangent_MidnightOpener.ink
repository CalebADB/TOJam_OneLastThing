
#Pers: Andy
I'm heading to bed.
->Start

=== Start ===
 * [Is she going to go to bed? # Pers: Alex]
    # Pers: Alex
    You're going to bed? 
    # Pers: Andy
    Yeah why do you ask?
 * [Are they okay? # Pers: Andy #SichMinReq: V_AndyExhausted_2.9]
    # Pers: Andy
    Are you doing okay?
    # Pers: Alex
    I mean I got all these dishes.
    Taking my time
    # Pers: Andy
    Just come to bed!
 + [Pick Tangent 1 #Pers: Alex]
    # StartTangent: MovingInTogether
 + [Pick Tangent 2 #Pers: Alex]
    # StartTangent: StartingAYoutubeSeries
 + [That bag wasn't the solution I needed. # Pers: Alex]
    -> ALoan
 - I'm not ready for bed. # Pers: Alex
#Pers: Andy
But I need to go to bed.
-> Start
->DONE
 
=== ALoan === 
NULL # Pers: Alex #StartTangent: ALoan
+ [I want to go to bed #Pers: Andy #SichMinReq: V_AndyExhausted_4.9]
    #Pers: Andy
    Ok, I need to go to bed.

-> Start
