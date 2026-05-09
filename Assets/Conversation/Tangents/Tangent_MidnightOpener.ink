
#Pers: Andy
I'm heading to bed.
->Start

=== Start ===
 * [Is she going to bed? # Pers: Player]
    # Pers: Player
    You're going to bed? 
    # Pers: Andy
    Yeah why do you ask?
 * [Are they okay? # Pers: Andy]
    # Pers: Andy
    Are you doing okay?
    # Pers: Player
    I mean I got all these dishes.
    Taking my time
    # Pers: Andy
    Just come to bed!
 + [Pick Tangent 1 #Pers: Player]
    # StartTangent: MovingInTogether
 + [Pick Tangent 2 #Pers: Player]
    # StartTangent: StartingAYoutubeSeries
 + [That bag wasn't the solution I needed. # Pers: Player]
    ->ALoan
 - I'm not ready for bed. # Pers: Player
#Pers: Andy
But I need to go to bed.
-> Start
->DONE
 
=== Beat_AndyHeadsToBed ===
# Pers: Andy
Alright Goodnight
-> DONE

=== ALoan ===
+ [I want to go to bed #Pers: Andy]
    #Pers: Andy
    Ok, I need to go to bed.
->Start
 
 