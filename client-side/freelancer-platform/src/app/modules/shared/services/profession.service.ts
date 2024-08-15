import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map, Observable, of, switchMap, tap} from 'rxjs';
import { Profession, Skill } from '../models/profession.mode';

@Injectable({
  providedIn: 'root'
})
export class ProfessionService {

  httpOptions = {
    headers: { 'Content-Type': 'application/json' }
  };

  private professions: Profession[] = []

  constructor(private httpClient: HttpClient) { }

  get(): Observable<Profession[]> {
    if (this.professions.length > 0) {
      return of(this.professions);
    } else {
      return this.httpClient.get<Profession[]>(`api/profession`).pipe(
        tap(professions => this.professions = professions)
      );
    }
  }

  getSkills(professionId: string): Observable<Skill[]> {
    return this.get().pipe(
      map(professions => {
        const profession = professions.find(p => p.id === professionId);
        if (!profession) {
          throw new Error(`Profession with id ${professionId} not found`);
        }
        return profession.skills;
      })
    );
  }

}
