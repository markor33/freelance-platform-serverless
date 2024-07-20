export class Language {
    id: number = 0;
    name: string = '';
    shortName: string = '';
}

export class LanguageKnowledge {
    language: Language = new Language();
    profiencyLevel: LanguageProficiencyLevel = LanguageProficiencyLevel.BASIC;
}

export enum LanguageProficiencyLevel {
    BASIC,
    CONVERSATIONAL,
    FLUENT,
    NATIVE
}