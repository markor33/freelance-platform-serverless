import { Question } from "./question.model";

export class Answer {
    id: string = '';
    questionId: string = '';
    question: Question = new Question('');
    text: string = '';
}