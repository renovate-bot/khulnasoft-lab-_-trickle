export interface ListDetails {
  id: number;
  name: string;
  description?: string;
  licenseId?: number;
  syntaxIds: number[];
  languageIds: number[];
  tagIds: number[];
  viewUrls: ViewUrl[];
  homeUrl?: string;
  onionUrl?: string;
  policyUrl?: string;
  submissionUrl?: string;
  issuesUrl?: string;
  forumUrl?: string;
  chatUrl?: string;
  emailAddress?: string;
  donateUrl?: string;
  maintainerIds: number[];
  upstreamTrickleIds: number[];
  forkTrickleIds: number[];
  includedInTrickleIds: number[];
  includesTrickleIds: number[];
  dependencyTrickleIds: number[];
  dependentTrickleIds: number[];
}

export interface ViewUrl {
  segmentNumber: number;
  primariness: number;
  url: string;
}
