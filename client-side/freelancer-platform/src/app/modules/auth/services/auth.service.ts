import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from '../models/user.model';
import { signInWithRedirect, signOut, fetchAuthSession, getCurrentUser } from "aws-amplify/auth"

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userSource = new BehaviorSubject<User | null>(null);
  public userObserver = this.userSource.asObservable();

  async init() {
    const { tokens } = await fetchAuthSession();
    if (!tokens) {
      return;
    }

    console.log(tokens);
    const { username, userId } = await getCurrentUser();
    const roles = tokens?.accessToken.payload['cognito:groups'] as string[]
    
    const user = new User(username, userId, roles[0]);
    this.userSource.next(user);
  }

  login() {
    signInWithRedirect();
  }

  register() {
    
  }

  logout(): void {
    this.userSource.next(null);
    signOut();
  }

  getUserRole(): string {
    return this.userSource.value?.role as string;
  }

  isLogged(): boolean {
    return this.userSource.value !== null;
  }

}
