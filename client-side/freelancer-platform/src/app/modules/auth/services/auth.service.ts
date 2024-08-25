import { Injectable } from '@angular/core';
import {BehaviorSubject, catchError, map, Observable, of} from 'rxjs';
import { User } from '../models/user.model';
import { signInWithRedirect, signOut, fetchAuthSession, getCurrentUser } from "aws-amplify/auth"
import {getRoleFromString, Role} from "../../shared/models/role.model";
import {HttpClient} from "@angular/common/http";
import {Registration} from "../models/registration.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userSource = new BehaviorSubject<User | null>(null);
  public userObserver = this.userSource.asObservable();

  private roleSource = new BehaviorSubject<Role | undefined>(undefined);
  public roleObserver = this.roleSource.asObservable();

  constructor(private httpClient: HttpClient) { }

  async init() {
    const { tokens } = await fetchAuthSession();
    if (!tokens) {
      return;
    }

    const { username, userId } = await getCurrentUser();
    const roles = tokens?.accessToken.payload['cognito:groups'] as string[] ?? [null]


    const user = new User(username, userId, roles[0]);
    this.roleSource.next(getRoleFromString(roles[0]));
    this.userSource.next(user);
  }

  login() {
    signInWithRedirect();
  }

  register(registration: Registration): Observable<any> {
    return this.httpClient.post<any>('api/identity-service/user', registration);
  }

  logout(): void {
    this.userSource.next(null);
    signOut();
  }

  hasRole(): boolean {
    return this.userSource.value?.role !== null
  }

  getUserRole(): string {
    return this.userSource.value?.role as string;
  }

  isLogged(): boolean {
    return this.userSource.value !== null;
  }

}
