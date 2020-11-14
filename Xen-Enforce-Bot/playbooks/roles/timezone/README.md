# Timezone

Set the system timezone.

## Requirements

This role relies on the `community.general` collection.

## Role Variables

| Variable   | Type   | Default            | Description         |
| ---------- | ------ | ------------------ | ------------------- |
| `timezone` | string | `America/New_York` | Timezone city name. |

## Example Playbook

```yaml
- hosts: all
  tasks:
    - include_role:
      name: timezone
```

## Author Information

[NC Anthro Society](https://ncanthrosociety.com)
