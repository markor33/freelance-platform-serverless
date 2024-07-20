export class FinishedContract {
    id: string = '';
    clientFeedback: Feedback = new Feedback();
    freelancerFeedback: Feedback = new Feedback();
}

export class Feedback {
    jobId: string = '';
    jobTitle: string = '';
    rating: number = 1;
    text: string = '';
}