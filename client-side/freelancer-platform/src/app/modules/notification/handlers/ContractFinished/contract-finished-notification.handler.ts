import { Injectable } from "@angular/core";
import { NotificationHandler } from "../notification-handler";
import { NotificationContent } from "../../models/notification-content.model";
import { ContractFinishedNotification } from "./contract-finished-notification.model";
import { Router } from "@angular/router";

@Injectable({
    providedIn: 'root'
})
export class ContractFinishedNotificationHandler implements NotificationHandler {

    constructor(private router: Router) { }

    getType(): string {
        return ContractFinishedNotification.name;
    }

    getContent(data: ContractFinishedNotification): NotificationContent {
        return {
            title: 'Client finished the contract',
            description: `Contract for '${data.JobTitle}' job is finished`
        }
    }

    handle(data: ContractFinishedNotification): void {
        this.router.navigate(['my-contracts']);
    }

}