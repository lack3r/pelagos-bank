# Incident response plan: the first hour

One page per team. Fill it in for the scenario on the slide, then keep the blank copy for your own
service. Short answers are fine: a name, a time, a sentence. Anything you cannot answer is a gap to fix
before the real thing.

**Service:** ______________________ **Team:** ______________________ **Plan written on:** ____________

## 1. Roles: who does what

| Role | Name (and a deputy) | Their first job |
|---|---|---|
| Incident lead (decides, keeps the timeline) | | |
| Engineer on call (investigates, contains) | | |
| Security contact (severity, evidence, outside help) | | |
| Data protection / compliance (the legal clocks) | | |
| Communications (staff, customers, press) | | |
| Management (approves big calls: shut down, pay, disclose) | | |

## 2. The first hour

- [ ] Minute 0: who raised it, when, how. Start a written timeline now (time, what, who).
- [ ] Minute 5: incident lead named; a call or channel opened just for this incident.
- [ ] Minute 15: first assessment: what is affected, is it still happening, how bad could it get.
- [ ] Minute 30: containment decision made (below), and who approved it.
- [ ] Minute 45: the legal clocks checked (section 5); management told.
- [ ] Minute 60: next update time agreed; everyone knows their next task.

## 3. Containment: stop it getting worse

| Option | What it costs us | Who can approve it | Chosen? |
|---|---|---|---|
| Turn off the affected feature or endpoint | | | |
| Block the account, token or IP address | | | |
| Revoke and rotate keys and secrets | | | |
| Ship the hotfix (with a test that proves it) | | | |

## 4. Evidence to keep before anything is cleaned up

- [ ] Application and access logs for the whole window (copy them before they rotate)
- [ ] Database records touched: what changed, from what, to what
- [ ] The deployed version and configuration at the time
- [ ] Copies of any messages from the attacker or about the incident
- Where the copies are kept, and who has access: ______________________

## 5. Who must be told, and by when

| Who | Rule or reason | Deadline | Owner | Done at |
|---|---|---|---|---|
| Data protection supervisory authority | GDPR Art. 33, if personal data is breached | 72 hours from becoming aware, where feasible | | |
| The people whose data it is | GDPR Art. 34, if the risk to them is high | Without undue delay | | |
| Financial regulator (financial entities) | DORA, major ICT-related incident | Initial notice 4 hours after classifying it as major, at most 24 hours after becoming aware | | |
| Bank or payment partners | Contract | | | |
| Police / national CSIRT | If a crime or a wider threat | | | |
| Insurer | Policy terms | | | |

## 6. What customers hear

Draft the first message in plain words: what happened, what it means for them, what we are doing,
what they should do, where to get help. No guesses presented as facts, no blame.

> ______________________________________________________________________________
>
> ______________________________________________________________________________

## 7. After it is over

- [ ] Blameless review within ______ days: what happened, why it was possible, why we did not see it sooner
- [ ] Root cause fixed for the whole class, not only this instance, with a regression test
- [ ] Detection added: the alert that would have caught it earlier
- [ ] This plan updated with what was missing
