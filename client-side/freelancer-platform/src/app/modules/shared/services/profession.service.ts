import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Profession, Skill } from '../models/profession.mode';

@Injectable({
  providedIn: 'root'
})
export class ProfessionService {

  httpOptions = {
    headers: { 'Content-Type': 'application/json' }
  };

  constructor(private httpClient: HttpClient) { }

  get(): Observable<Profession[]> {
    return this.httpClient.get<Profession[]>(`api/freelancer/profession`);
  }

  getSkills(professionId: string): Observable<Skill[]> {
    return this.httpClient.get<Skill[]>(`api/freelancer/profession/${professionId}/skills`);
  }

}
