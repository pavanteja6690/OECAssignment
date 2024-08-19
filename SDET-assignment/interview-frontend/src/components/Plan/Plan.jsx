import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import {
  addProcedureToPlan,
  getPlanProcedureUsers,
  getPlanProcedures,
  getProcedures,
  getUsers,
} from "../../api/api";
import Layout from "../Layout/Layout";
import ProcedureItem from "./ProcedureItem/ProcedureItem";
import PlanProcedureItem from "./PlanProcedureItem/PlanProcedureItem";

const Plan = () => {
  let { id } = useParams();
  const [procedures, setProcedures] = useState([]);
  const [planProcedures, setPlanProcedures] = useState([]);
  const [planProceduresSelectedUsers, setPlanProceduresSelectedUsers] = useState({});
  const [users, setUsers] = useState([]);

  useEffect(() => {
    (async () => {
      const procedures = await getProcedures();
      const planProcedures = await getPlanProcedures(id);
      const planProcedureUsers = await getPlanProcedureUsers(id);
      const users = await getUsers();

      const userOptions = users.map((u) => ({ label: u.name, value: u.userId }));

      const mappedUsers = planProcedureUsers.reduce((acc, item) => {
        acc[item.procedureId] = item.planProcedureUsers.map(user => {
          const userOption = userOptions.find(uo => uo.value === user.planProcedureUsersUserId);
          return userOption ? { label: userOption.label, value: userOption.value } : null;
        }).filter(Boolean); 
        return acc;
      }, {});

      setUsers(userOptions);
      setProcedures(procedures);
      setPlanProcedures(planProcedures);
      setPlanProceduresSelectedUsers(mappedUsers);
    })();
  }, [id]);

    const handleAddProcedureToPlan = async (procedure) => {
        const hasProcedureInPlan = planProcedures.some((p) => p.procedureId === procedure.procedureId);
        if (hasProcedureInPlan) return;

        await addProcedureToPlan(id, procedure.procedureId);
        setPlanProcedures((prevState) => {
            return [
                ...prevState,
                {
                    planId: id,
                    procedureId: procedure.procedureId,
                    procedure: {
                        procedureId: procedure.procedureId,
                        procedureTitle: procedure.procedureTitle,
                    },
                },
            ];
        });
    };

  return (
    <Layout>
      <div className="container pt-4">
        <div className="d-flex justify-content-center">
          <h2>OEC Interview Frontend</h2>
        </div>
        <div className="row mt-4">
          <div className="col">
            <div className="card shadow">
              <h5 className="card-header">Repair Plan</h5>
              <div className="card-body">
                <div className="row">
                  <div className="col">
                    <h4>Procedures</h4>
                    <div>
                      {procedures.map((p) => (
                        <ProcedureItem
                          key={p.procedureId}
                          procedure={p}
                          handleAddProcedureToPlan={handleAddProcedureToPlan}
                          planProcedures={planProcedures}
                        />
                      ))}
                    </div>
                  </div>
                  <div className="col">
                    <h4>Added to Plan</h4>
                    <div>
                      {planProcedures.map((p) => (
                        <PlanProcedureItem
                          key={p.procedure.procedureId}
                          procedure={p.procedure}
                          planId={id}
                          users={users}
                          planProceduresSelectedUsers={planProceduresSelectedUsers[p.procedure.procedureId] || []}
                        />
                      ))}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default Plan;
