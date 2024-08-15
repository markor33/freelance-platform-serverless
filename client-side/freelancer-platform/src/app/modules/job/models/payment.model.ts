export class Payment {
    amount: number = 0.0;
    currency: string = '';
    type: PaymentType = PaymentType.FIXED_RATE;
}

export enum PaymentType {
    FIXED_RATE,
    HOURLY_RATE
}
